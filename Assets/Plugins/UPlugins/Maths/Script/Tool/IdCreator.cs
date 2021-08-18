/////////////////////////////////////////////////////////////////////////////
//
//  Script : IdCreator.cs
//  Info   : 64位 ID 生成器
//  Author : snowflake
//  E-mail : ls9512@vip.qq.com
//
//  Copyright : https://www.cnblogs.com/xululublog/p/7047281.html
//
/////////////////////////////////////////////////////////////////////////////
using System;
using Random = System.Random;

namespace Aya.Maths
{
    /// <summary>
    /// 64位ID生成器,最高位为符号位,始终为0,可用位数63.
    /// 实例编号占10位,范围为0-1023
    /// 时间戳和索引共占53位
    /// </summary>
    public sealed class IdCreator 
    {
        private static readonly Random Rand = new Random();
        private static readonly IdCreator _default = new IdCreator();

        private readonly long _instanceID;                              // 实例编号
        private readonly int _indexBitLength;                           // 索引可用位数
        private readonly long _tsMax = 0;                               // 时间戳最大值
        private readonly long _indexMax = 0;
        private readonly object _lock = new object();

        private long _timestamp = 0;                                    // 当前时间戳
        private long _index = 0;                                        // 索引/计数器

        /// <summary>
        /// 构造
        /// </summary>
        /// <param name="instanceID">实例编号(0-1023)</param>
        /// <param name="indexBitLength">索引可用位数(1-32).每秒可生成ID数等于2的indexBitLength次方.大并发情况下,当前秒内ID数达到最大值时,将使用下一秒的时间戳,不影响获取ID.</param>
        /// <param name="initTimestamp">初始化时间戳,精确到秒.当之前同一实例生成ID的timestamp值大于当前时间的时间戳时,
        /// 有可能会产生重复ID(如持续一段时间的大并发请求).设置initTimestamp比最后的时间戳大一些,可避免这种问题</param>
        public IdCreator(int instanceID, int indexBitLength, long? initTimestamp = null)
        {
            if (instanceID < 0)
            {
                //这里给每个实例随机生成个实例编号
                _instanceID = Rand.Next(0, 1024);
            }
            else
            {
                _instanceID = instanceID % 1024;
            }

            if (indexBitLength < 1)
            {
                _indexBitLength = 1;
            }
            else if (indexBitLength > 32)
            {
                _indexBitLength = 32;
            }
            else
            {
                _indexBitLength = indexBitLength;
            }
            _tsMax = Convert.ToInt64(new string('1', 53 - indexBitLength), 2);
            _indexMax = Convert.ToInt64(new string('1', indexBitLength), 2);

            if (initTimestamp != null)
            {
                _timestamp = initTimestamp.Value;
            }
        }

        /// <summary>
        /// 默认每实例每秒生成65536个ID,从1970年1月1日起,累计可使用4358年
        /// </summary>
        /// <param name="instanceID">实例编号(0-1023)</param>
        public IdCreator(int instanceID) : this(instanceID, 16)
        {
        }

        /// <summary>
        /// 默认每秒生成65536个ID,从1970年1月1日起,累计可使用4358年
        /// </summary>
        public IdCreator() : this(-1)
        {
        }

        /// <summary>
        /// 生成64位ID
        /// </summary>
        /// <returns></returns>
        public long Create()
        {
            long id = 0;

            lock (_lock)
            {
                // 增加时间戳部分
                var startTime  = TimeZone.CurrentTimeZone.ToUniversalTime(new DateTime(1970, 1, 1));
                var nowTime = DateTime.UtcNow;
                var timeStamp = (long)(nowTime - startTime).TotalSeconds;
                var ts = timeStamp / 1000;

                // 如果超过时间戳允许的最大值,从0开始
                ts = ts % _tsMax;
                // 腾出后面部分,给实例编号和索引编号使用
                id = ts << (10 + _indexBitLength);

                // 增加实例部分
                id = id | (_instanceID << _indexBitLength);

                // 获取计数
                if (_timestamp < ts)
                {
                    _timestamp = ts;
                    _index = 0;
                }
                else
                {
                    if (_index > _indexMax)
                    {
                        _timestamp++;
                        _index = 0;
                    }
                }

                id = id | _index;

                _index++;
            }

            return id;
        }

        /// <summary>
        /// 获取当前实例的时间戳
        /// </summary>
        public long CurrentTimestamp
        {
            get
            {
                return _timestamp;
            }
        }

        /// <summary>
        /// 默认每实例每秒生成65536个ID,从1970年1月1日起,累计可使用4358年
        /// </summary>
        public static IdCreator Default
        {
            get
            {
                return _default;
            }
        }
    }
}
