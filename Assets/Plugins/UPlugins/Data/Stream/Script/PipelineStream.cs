/////////////////////////////////////////////////////////////////////////////
//
//  Script : PipelineStream.cs
//  Info   : 管道通讯流 允许同步化的形式在两个不同线程的传递数据
//  Author : CatLib
//  E-mail : ls9512@vip.qq.com
//
//  Copyright : Aya Game Studio 2019
//
/////////////////////////////////////////////////////////////////////////////
using System;
using System.IO;
using System.Threading;
using Aya.Common;
using Aya.Extension;

namespace Aya.Data
{
    public class PipelineStream : WrapperStream
    {
        /// <summary>
        /// 可以被读取的长度
        /// </summary>
        private volatile int _count;

        /// <summary>
        /// 容量
        /// </summary>
        private readonly int _capacity;

        /// <summary>
        /// 休眠时间
        /// </summary>
        private readonly int _sleep;

        /// <summary>
        /// 环形缓冲区
        /// </summary>
        private readonly RingBuffer _ringBuffer;

        /// <summary>
        /// 当完成读取后触发
        /// </summary>
        public event Action<Stream> OnRead;

        /// <summary>
        /// 是否已经被释放了
        /// </summary>
        private volatile bool _disabled;

        /// <summary>
        /// 是否已经关闭流了
        /// </summary>
        private volatile bool _closed;

        /// <summary>
        /// 是否可以被读取
        /// </summary>
        public override bool CanRead => _count > 0 && !_disabled;

        /// <summary>
        /// 是否可以被写入
        /// </summary>
        public override bool CanWrite => _count < _capacity && !_closed;
        /// <summary>
        /// 当前流的位置
        /// </summary>
        private long _position;

        /// <summary>
        /// 流位置
        /// </summary>
        public override long Position
        {
            get { return _position; }
            set { throw new NotSupportedException(); }
        }

        /// <summary>
        /// 流的长度
        /// </summary>
        private long length;

        /// <summary>
        /// 流的长度
        /// </summary>
        public override long Length => length;

        /// <summary>
        /// 是否能够设定偏移量
        /// </summary>
        public override bool CanSeek => false;

        /// <summary>
        /// 是否已经关闭了流
        /// </summary>
        public bool Closed => _closed;

        /// <summary>
        /// 管道通讯流
        /// </summary>
        /// <param name="capacity">缓冲区容量</param>
        /// <param name="sleep">线程休眠时间</param>
        public PipelineStream(int capacity = 4096, int sleep = 1)
        {
            this._capacity = capacity.ToPrime();
            this._sleep = Math.Max(0, sleep);
            _ringBuffer = new RingBuffer(this._capacity, false);
        }

        /// <summary>
        /// GC回收时
        /// </summary>
        ~PipelineStream()
        {
            Dispose(!_disabled);
        }

        /// <summary>
        /// 设定流位置（不支持）
        /// </summary>
        /// <param name="offset">偏移量</param>
        /// <param name="origin">偏移方向</param>
        /// <returns></returns>
        public override long Seek(long offset, SeekOrigin origin)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// 设定流的长度
        /// </summary>
        /// <param name="value">长度</param>
        public override void SetLength(long value)
        {
            length = Math.Max(0, value);
        }

        /// <summary>
        /// 刷新缓冲区
        /// </summary>
        public override void Flush()
        {
            // ignore
        }

        /// <summary>
        /// 将流中的数据读取到指定缓冲区
        /// </summary>
        /// <param name="buffer">指定缓冲区</param>
        /// <param name="offset">缓冲区起始偏移量</param>
        /// <param name="count">读取的长度</param>
        /// <returns>实际读取的长度</returns>
        public override int Read(byte[] buffer, int offset, int count)
        {
            while (true)
            {
                while (this._count <= 0)
                {
                    AssertDisabled();
                    if (_closed)
                    {
                        return 0;
                    }
                    Thread.Sleep(_sleep);
                }

                AssertDisabled();

                lock (_ringBuffer.SyncRoot)
                {
                    AssertDisabled();
                    if (this._count <= 0)
                    {
                        if (_closed)
                        {
                            return 0;
                        }
                        continue;
                    }

                    try
                    {
                        var read = _ringBuffer.Read(buffer, offset, count);
                        this._count -= read;
                        _position += read;

                        OnRead?.Invoke(this);

                        return read;
                    }
                    finally
                    {
                        Error.Check<Exception>(this._count >= 0);
                    }
                }
            }
        }

        /// <summary>
        /// 将指定缓冲区数据写入流中
        /// </summary>
        /// <param name="buffer">指定缓冲区</param>
        /// <param name="offset">缓冲区起始偏移量</param>
        /// <param name="count">写入的长度</param>
        public override void Write(byte[] buffer, int offset, int count)
        {
            while (true)
            {
                while ((_capacity - this._count) < count)
                {
                    AssertDisabled();
                    AssertClosed();

                    Thread.Sleep(_sleep);
                }

                AssertDisabled();
                AssertClosed();

                lock (_ringBuffer.SyncRoot)
                {
                    AssertDisabled();
                    AssertClosed();

                    if ((_capacity - this._count) < count)
                    {
                        continue;
                    }

                    Error.Check<Exception>(_ringBuffer.Write(buffer, offset, count) == count);
                    this._count += count;
                    return;
                }
            }
        }

        /// <summary>
        /// 关闭流
        /// </summary>
        public override void Close()
        {
            if (_closed)
            {
                return;
            }

            lock (_ringBuffer.SyncRoot)
            {
                _closed = true;
            }
        }

        /// <summary>
        /// 断言关闭
        /// </summary>
        protected void AssertClosed()
        {
            if (_closed)
            {
                throw new ObjectDisposedException(nameof(PipelineStream), $"Stream is {nameof(_closed)} Cannot write");
            }
        }

        /// <summary>
        /// 断言释放
        /// </summary>
        protected void AssertDisabled()
        {
            if (_disabled)
            {
                throw new ObjectDisposedException(nameof(PipelineStream), $"Stream is {_disabled}");
            }
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        /// <param name="disposing">是否进行释放</param>
        protected override void Dispose(bool disposing)
        {
            if (!disposing || _disabled)
            {
                return;
            }

            lock (_ringBuffer.SyncRoot)
            {
                if (_disabled)
                {
                    return;
                }

                try
                {
                    _disabled = true;
                    _closed = true;
                    _ringBuffer.Dispose();
                }
                finally
                {
                    base.Dispose(true);
                }
            }
        }
    }
}
