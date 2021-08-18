/////////////////////////////////////////////////////////////////////////////
//
//  Script : CombineStream.cs
//  Info   : 组合流，允许将多个不同的流组合成一个流，这两个流可以分别为流式传输和非流式传输。
//  Author : CatLib
//  E-mail : ls9512@vip.qq.com
//
//  Copyright : Aya Game Studio 2019
//
/////////////////////////////////////////////////////////////////////////////
using System;
using System.IO;
using Aya.Common;

namespace Aya.Data
{
    public class CombineStream : WrapperStream
    {
        /// <summary>
        /// 全局游标位置
        /// </summary>
        private long _globalPosition;

        /// <summary>
        /// 当前所处的流
        /// </summary>
        private int _index;

        /// <summary>
        /// 组合流
        /// </summary>
        private readonly Stream[] _streams;

        /// <summary>
        /// 组合流的长度
        /// </summary>
        private long _length;

        /// <summary>
        /// 组合流的长度
        /// </summary>
        public override long Length
        {
            get
            {
                if (_length >= 0)
                {
                    return _length;
                }

                _length = 0;
                foreach (var stream in _streams)
                {
                    _length += stream.Length;
                }
                return _length;
            }
        }

        /// <summary>
        /// 是否能够偏移
        /// </summary>
        public override bool CanSeek
        {
            get
            {
                foreach (var stream in _streams)
                {
                    if (!stream.CanSeek)
                    {
                        return false;
                    }
                }
                return true;
            }
        }

        /// <summary>
        /// 获取当前偏移量
        /// </summary>
        public override long Position
        {
            get { return _globalPosition; }
            set { Seek(value, SeekOrigin.Begin); }
        }

        /// <summary>
        /// 是否是可读的
        /// </summary>
        public override bool CanRead
        {
            get
            {
                foreach (var stream in _streams)
                {
                    if (!stream.CanRead)
                    {
                        return false;
                    }
                }
                return true;
            }
        }

        /// <summary>
        /// 是否是可写的
        /// </summary>
        public override bool CanWrite => false;

        /// <summary>
        /// 组合流关闭时是否自动关闭流
        /// </summary>
        private readonly bool autoClosed;

        /// <summary>
        /// 构建一个组合流实例，允许将两个不同的流组合成一个流
        /// </summary>
        /// <param name="left">流</param>
        /// <param name="right">流</param>
        /// <param name="closed">当组合流释放时是否自动关闭其中的流</param>
        public CombineStream(Stream left, Stream right, bool closed = false)
            : this(new[] { left, right }, closed)
        {
        }

        /// <summary>
        /// 构建一个组合流实例，允许将多个流组合成一个流
        /// </summary>
        /// <param name="source">流</param>
        /// <param name="closed">当组合流释放时是否自动关闭其中的流</param>
        public CombineStream(Stream[] source, bool closed = false)
        {
            _index = 0;
            _streams = source;
            _length = -1;
            autoClosed = closed;
        }

        /// <summary>
        /// 设定位置偏移
        /// </summary>
        /// <param name="offset">偏移量</param>
        /// <param name="origin">偏移方向</param>
        /// <returns>当前偏移量</returns>
        public override long Seek(long offset, SeekOrigin origin)
        {
            if (!CanSeek)
            {
                throw new NotSupportedException($"{nameof(CombineStream)} not supported {nameof(Seek)}.");
            }

            long newGloablPosition;
            switch (origin)
            {
                case SeekOrigin.Begin:
                    newGloablPosition = offset;
                    break;
                case SeekOrigin.Current:
                    newGloablPosition = _globalPosition + offset;
                    break;
                case SeekOrigin.End:
                    newGloablPosition = Length + offset;
                    break;
                default:
                    throw new NotSupportedException($"Not support {nameof(SeekOrigin)}: {origin}");
            }

            if (newGloablPosition < 0 || newGloablPosition > Length)
            {
                throw new ArgumentOutOfRangeException($"{nameof(offset)} must large than zero or small then {nameof(Length)}");
            }

            long localPosition = 0;
            var newIndex = _index = CalculatedIndex(newGloablPosition, ref localPosition);

            _streams[newIndex].Seek(localPosition, SeekOrigin.Begin);
            while (++newIndex < _streams.Length)
            {
                _streams[newIndex].Seek(0, SeekOrigin.Begin);
            }

            return _globalPosition = newGloablPosition;
        }

        /// <summary>
        /// 计算偏移下标
        /// </summary>
        /// <param name="globalPosition">全局位置</param>
        /// <param name="localPosition">本地偏移量</param>
        protected int CalculatedIndex(long globalPosition, ref long localPosition)
        {
            long length = 0;
            for (var i = 0; i < _streams.Length; i++)
            {
                length += _streams[i].Length;
                if (globalPosition > length)
                {
                    continue;
                }

                localPosition = _streams[i].Length - (length - globalPosition);
                return i;
            }

            throw new Exception($"Failed to determine {nameof(localPosition)}");
        }

        /// <summary>
        /// 读取组合流的数据到缓冲区
        /// </summary>
        /// <param name="buffer">缓冲区</param>
        /// <param name="offset">缓冲区偏移量</param>
        /// <param name="count">希望读取的长度</param>
        /// <returns>实际读取的长度</returns>
        public override int Read(byte[] buffer, int offset, int count)
        {
            Error.Check<ArgumentNullException>(buffer != null);
            Error.Check<ArgumentOutOfRangeException>(offset >= 0);
            Error.Check<ArgumentOutOfRangeException>(count >= 0);
            Error.Check<ArgumentOutOfRangeException>(buffer.Length - offset >= count);

            var result = 0;
            do
            {
                var read = _streams[_index].Read(buffer, offset, count);
                if (read <= 0 && _index < _streams.Length - 1)
                {
                    _index++;
                    continue;
                }

                if (read <= 0)
                {
                    break;
                }

                count -= read;
                offset += read;
                _globalPosition += read;
                result += read;
            } while (count > 0);

            return result;
        }

        /// <summary>
        /// 写入数据到组合流
        /// </summary>
        /// <param name="buffer">缓冲区</param>
        /// <param name="offset">偏移量</param>
        /// <param name="count"></param>
        public override void Write(byte[] buffer, int offset, int count)
        {
            throw new NotSupportedException($"{nameof(CombineStream)} not supported {nameof(Write)}.");
        }

        /// <summary>
        /// 设定流的长度
        /// </summary>
        /// <param name="value">长度</param>
        public override void SetLength(long value)
        {
            throw new NotSupportedException($"{nameof(CombineStream)} not supported {nameof(SetLength)}.");
        }

        /// <summary>
        /// Flush Stream
        /// </summary>
        public override void Flush()
        {
            throw new NotSupportedException($"{nameof(CombineStream)} not supported {nameof(Flush)}.");
        }

        /// <summary>
        /// 当组合流释放时
        /// </summary>
        /// <param name="disposing"></param>
        protected override void Dispose(bool disposing)
        {
            try
            {
                if (!autoClosed)
                {
                    return;
                }

                foreach (var stream in _streams)
                {
                    stream?.Dispose();
                }
            }
            finally
            {
                base.Dispose(disposing);
            }
        }
    }
}
