/////////////////////////////////////////////////////////////////////////////
//
//  Script   : RingBuffer.cs
//  Info     : 环型缓冲区
//  Author   : CatLib
//  E-mail   : ls9512@vip.qq.com
//
//  Copyright : CatLib 2018
//
/////////////////////////////////////////////////////////////////////////////
using System;
using System.Threading;
using Aya.Common;
using Aya.Extension;

namespace Aya.Data
{
    public class RingBuffer : IDisposable
    {
        /// <summary>
        /// 容量
        /// </summary>
        private readonly long _capacity;

        /// <summary>
        /// 缓冲区容量
        /// </summary>
        public int Capacity => (int)_capacity;

        /// <summary>
        /// 缓冲区大小
        /// </summary>
        private readonly byte[] _buffer;

        /// <summary>
        /// 原始数组是否可以返回给开发者
        /// </summary>
        private readonly bool _exposable;

        /// <summary>
        /// 写入的游标
        /// </summary>
        private long _write;

        /// <summary>
        /// 读取的游标
        /// </summary>
        private long _read;

        /// <summary>
        /// 遮罩层
        /// <para>为了快速计算出,环回中的写入点</para>
        /// </summary>
        private readonly long _mask;

        /// <summary>
        /// 同步锁
        /// </summary>
        private object _syncRoot;

        /// <summary>
        /// 同步锁
        /// </summary>
        public object SyncRoot
        {
            get
            {
                if (_syncRoot == null)
                {
                    Interlocked.CompareExchange(ref _syncRoot, new object(), null);
                }
                return _syncRoot;
            }
        }

        /// <summary>
        /// 可写容量
        /// </summary>
        public int WritableCapacity => (int) GetCanWriteSize();

        /// <summary>
        /// 可读容量
        /// </summary>
        public int ReadableCapacity => (int) GetCanReadSize();

        /// <summary>
        /// 构建一个新的环型缓冲区实例
        /// </summary>
        /// <param name="capacity">容量,将为临近2的次方（向上取）</param>
        /// <param name="exposable">是否可以访问内部数组</param>
        public RingBuffer(int capacity = 8192, bool exposable = true)
        {
            Error.Check<ArgumentOutOfRangeException>(capacity > 0);
            _buffer = new byte[this._capacity = capacity.ToPrime()];
            _mask = this._capacity - 1;
            _write = 0;
            _read = 0;
            this._exposable = exposable;
        }

        /// <summary>
        /// 是否可以进行读取
        /// </summary>
        /// <param name="count">指定的长度</param>
        public bool CanRead(int count = 1)
        {
            Error.Check<ArgumentOutOfRangeException>(_capacity > 0);
            return GetCanReadSize() >= count;
        }

        /// <summary>
        /// 是否可以进行写入
        /// </summary>
        /// <param name="count">指定的长度</param>
        public bool CanWrite(int count = 1)
        {
            Error.Check<ArgumentOutOfRangeException>(_capacity > 0);
            return GetCanWriteSize() >= count;
        }

        /// <summary>
        /// 获取环型缓冲区的原始数组
        /// </summary>
        /// <returns>原始数组</returns>
        public byte[] GetBuffer()
        {
            if (!_exposable)
            {
                throw new UnauthorizedAccessException("Unable to access original array");
            }
            return _buffer;
        }

        /// <summary>
        /// 将可以读取的数据全部返回
        /// </summary>
        /// <returns>可以读取的数据</returns>
        public byte[] Read()
        {
            var buffer = MakeReadableBuffer();
            if (buffer == null)
            {
                return new byte[0];
            }

            Read(buffer);
            return buffer;
        }

        /// <summary>
        /// 将数据读取到<paramref name="buffer"/>中
        /// </summary>
        /// <param name="buffer">输出的数据</param>
        /// <returns>实际输出的长度</returns>
        public int Read(byte[] buffer)
        {
            Error.Check<ArgumentNullException>(buffer != null);
            return Read(buffer, 0, buffer.Length);
        }

        /// <summary>
        /// 将数据读取到<paramref name="buffer"/>中
        /// </summary>
        /// <param name="buffer">输出的数据</param>
        /// <param name="offset">输出数组偏移多少作为起始</param>
        /// <returns>实际输出的长度</returns>
        public int Read(byte[] buffer, int offset)
        {
            Error.Check<ArgumentNullException>(buffer != null);
            return Read(buffer, offset, buffer.Length - offset);
        }

        /// <summary>
        /// 将数据读取到<paramref name="buffer"/>中
        /// </summary>
        /// <param name="buffer">输出的数据</param>
        /// <param name="offset">输出数组偏移多少作为起始</param>
        /// <param name="count">输出的长度</param>
        /// <returns>实际输出的长度</returns>
        public int Read(byte[] buffer, int offset, int count)
        {
            var readSize = Peek(buffer, offset, count);
            _read += readSize;
            return readSize;
        }

        /// <summary>
        /// 将环型缓冲区的数据全部返回，但是不前移读取位置
        /// </summary>
        /// <returns>实际输出的长度</returns>
        public byte[] Peek()
        {
            var buffer = MakeReadableBuffer();
            if (buffer == null)
            {
                return new byte[0];
            }

            Peek(buffer, 0, buffer.Length);
            return buffer;
        }

        /// <summary>
        /// 将环型缓冲区的数据读取到<paramref name="buffer"/>中，但是不前移读取位置
        /// </summary>
        /// <param name="buffer">输出的数据</param>
        /// <returns>实际输出的长度</returns>
        public int Peek(byte[] buffer)
        {
            Error.Check<ArgumentNullException>(buffer != null);
            return Peek(buffer, 0, buffer.Length);
        }

        /// <summary>
        /// 将环型缓冲区的数据读取到<paramref name="buffer"/>中，但是不前移读取位置
        /// </summary>
        /// <param name="buffer">输出的数据</param>
        /// <param name="offset">输出数组偏移多少作为起始</param>
        /// <returns>实际输出的长度</returns>
        public int Peek(byte[] buffer, int offset)
        {
            Error.Check<ArgumentNullException>(buffer != null);
            return Peek(buffer, offset, buffer.Length - offset);
        }

        /// <summary>
        /// 将环型缓冲区的数据读取到<paramref name="buffer"/>中，但是不前移读取位置
        /// </summary>
        /// <param name="buffer">输出的数据</param>
        /// <param name="offset">输出数组偏移多少作为起始</param>
        /// <param name="count">输出的长度</param>
        /// <returns>实际输出的长度</returns>
        public int Peek(byte[] buffer, int offset, int count)
        {
            Error.Check<ArgumentNullException>(buffer != null);
            Error.Check<ArgumentOutOfRangeException>(offset >= 0);
            Error.Check<ArgumentOutOfRangeException>(count >= 0);
            Error.Check<ArgumentOutOfRangeException>((buffer.Length - offset) >= count);

            var readSize = GetCanReadSize();
            if (readSize > count)
            {
                readSize = count;
            }

            if (readSize <= 0)
            {
                return 0;
            }

            var nextReadPos = _read + readSize;

            var realReadPos = _read & _mask;
            var realNextReadPos = nextReadPos & _mask;

            if (realNextReadPos >= realReadPos)
            {
                Buffer.BlockCopy(this._buffer, (int)realReadPos, buffer, offset, (int)readSize);
            }
            else
            {
                var tail = (int)(_capacity - realReadPos);
                Buffer.BlockCopy(this._buffer, (int)realReadPos, buffer, offset, tail);

                if (readSize - tail > 0)
                {
                    Buffer.BlockCopy(this._buffer, 0, buffer, offset + tail, (int)readSize - tail);
                }
            }

            return (int)readSize;
        }

        /// <summary>
        /// 将数据写入到环型缓冲区
        /// </summary>
        /// <param name="buffer">写入的数据</param>
        /// <returns>实际被写入的长度</returns>
        public int Write(byte[] buffer)
        {
            Error.Check<ArgumentNullException>(buffer != null);
            return Write(buffer, 0, buffer.Length);
        }

        /// <summary>
        /// 将数据写入到环型缓冲区
        /// </summary>
        /// <param name="buffer">写入的数据</param>
        /// <param name="offset">偏移多少数据开始写入</param>
        /// <returns>实际被写入的长度</returns>
        public int Write(byte[] buffer, int offset)
        {
            Error.Check<ArgumentNullException>(buffer != null);
            return Write(buffer, offset, buffer.Length - offset);
        }

        /// <summary>
        /// 将数据写入到环型缓冲区
        /// </summary>
        /// <param name="buffer">写入的数据</param>
        /// <param name="offset">偏移多少数据开始写入</param>
        /// <param name="count">写入的长度</param>
        /// <returns>实际被写入的长度</returns>
        public int Write(byte[] buffer, int offset, int count)
        {
            Error.Check<ArgumentNullException>(buffer != null);
            Error.Check<ArgumentOutOfRangeException>(offset >= 0);
            Error.Check<ArgumentOutOfRangeException>(count >= 0);
            Error.Check<ArgumentOutOfRangeException>((buffer.Length - offset) >= count);

            // 得到可以被写入的字节流大小
            var writeSize = GetCanWriteSize();
            if (writeSize > count)
            {
                writeSize = count;
            }

            if (writeSize <= 0)
            {
                return 0;
            }

            // 当前输入结束后下一次开始的写入点
            var nextWritePos = _write + writeSize;

            // 通过&运算遮罩快速获得环回中的写入点
            var realWritePos = _write & _mask;
            var realNextWritePos = nextWritePos & _mask;

            if (realNextWritePos >= realWritePos)
            {
                // 不会产生环回,只需要单纯写入
                Buffer.BlockCopy(buffer, offset, this._buffer, (int)realWritePos, (int)writeSize);
            }
            else
            {
                // 从写入位置到buffer流尾部的长度
                var tail = (int)(_capacity - realWritePos);
                Buffer.BlockCopy(buffer, offset, this._buffer, (int)realWritePos, tail);

                if ((writeSize - tail) > 0)
                {
                    Buffer.BlockCopy(buffer, offset + tail, this._buffer, 0, (int)writeSize - tail);
                }
            }

            _write = nextWritePos;
            return (int)writeSize;
        }

        /// <summary>
        /// 清空缓冲区中的所有数据
        /// </summary>
        public void Flush()
        {
            _write = 0;
            _read = 0;
            Array.Clear(_buffer, 0, _buffer.Length);
        }

        /// <summary>
        /// 释放
        /// </summary>
        public void Dispose()
        {
            Flush();
        }

        /// <summary>
        /// 获取可以被读取的字节流大小
        /// </summary>
        /// <returns></returns>
        private long GetCanReadSize()
        {
            return _write - _read;
        }

        /// <summary>
        /// 得到可以被写入的字节流大小
        /// </summary>
        private long GetCanWriteSize()
        {
            return Math.Max(0, _capacity - GetCanReadSize());
        }

        /// <summary>
        /// 获取当前可读的buffer
        /// </summary>
        /// <returns>可以被读取的buffer</returns>
        private byte[] MakeReadableBuffer()
        {
            var readSize = GetCanReadSize();
            return readSize <= 0 ? null : new byte[readSize];
        }
    }

}
