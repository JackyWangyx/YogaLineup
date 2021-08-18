/////////////////////////////////////////////////////////////////////////////
//
//  Script : SegmentStream.cs
//  Info   : 分片流 可以用于包装指定分片的流。使指定分片的流访问起来就像传统流那样从开头到结尾。
//  Author : CatLib
//  E-mail : ls9512@vip.qq.com
//
//  Copyright : Aya Game Studio 2019
//
/////////////////////////////////////////////////////////////////////////////
using System;
using System.IO;

namespace Aya.Data
{
    public class SegmentStream : WrapperStream
    {
        /// <summary>
        /// 基础流的初始的位置
        /// </summary>
        private readonly long _initialPosition;

        /// <summary>
        /// 分片的大小
        /// </summary>
        private readonly long _partSize;

        /// <summary>
        /// 构造一个分片流
        /// </summary>
        /// <param name="stream">基础流</param>
        /// <param name="partSize">分片大小</param>
        public SegmentStream(Stream stream, long partSize = 0)
            : base(stream)
        {
            if (!stream.CanSeek)
            {
                throw new InvalidOperationException($"Base stream of {nameof(SegmentStream)} must be seekable");
            }

            _initialPosition = stream.Position;
            var remainingSize = stream.Length - stream.Position;

            if (partSize == 0 || remainingSize < partSize)
            {
                this._partSize = remainingSize;
            }
            else
            {
                this._partSize = partSize;
            }
        }

        /// <summary>
        /// 剩余长度
        /// </summary>
        private long RemainingSize => _partSize - Position;

        /// <summary>
        /// 流的长度
        /// </summary>
        public override long Length
        {
            get
            {
                var length = base.Length - _initialPosition;
                if (length > _partSize)
                {
                    length = _partSize;
                }
                return length;
            }
        }

        /// <summary>
        /// 当前流的位置
        /// </summary>
        public override long Position
        {
            get { return base.Position - _initialPosition; }
            set { base.Position = value; }
        }

        /// <summary>
        /// 设定流的位置
        /// </summary>
        /// <param name="offset">偏移量</param>
        /// <param name="origin">偏移方向</param>
        /// <returns>流的位置</returns>
        public override long Seek(long offset, SeekOrigin origin)
        {
            long position;
            switch (origin)
            {
                case SeekOrigin.Begin:
                    position = _initialPosition + offset;
                    break;
                case SeekOrigin.Current:
                    position = base.Position + offset;
                    break;
                case SeekOrigin.End:
                    position = base.Position + _partSize + offset;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(origin), origin, null);
            }

            if (position < _initialPosition)
            {
                position = _initialPosition;
            }
            else if (position > _initialPosition + _partSize)
            {
                position = _initialPosition + _partSize;
            }

            base.Seek(position, SeekOrigin.Begin);
            return Position;
        }

        /// <summary>
        /// 读取流的数据到指定缓冲区
        /// </summary>
        /// <param name="buffer">指定缓冲区</param>
        /// <param name="offset">缓冲区偏移量</param>
        /// <param name="count">希望读取的长度</param>
        /// <returns>实际读取的长度</returns>
        public override int Read(byte[] buffer, int offset, int count)
        {
            var bytesToRead = count < RemainingSize ? count : (int)RemainingSize;
            return bytesToRead < 0 ? 0 : base.Read(buffer, offset, bytesToRead);
        }

        /// <summary>
        /// 设定流的长度
        /// </summary>
        /// <param name="value"></param>
        public override void SetLength(long value)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// 将指定缓冲区的数据写入流中
        /// </summary>
        /// <param name="buffer">指定缓冲区</param>
        /// <param name="offset">缓冲区偏移量</param>
        /// <param name="count">写入数据的长度</param>
        public override void Write(byte[] buffer, int offset, int count)
        {
            throw new NotSupportedException();
        }
    }
}