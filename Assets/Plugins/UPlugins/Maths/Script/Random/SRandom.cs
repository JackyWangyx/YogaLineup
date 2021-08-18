/////////////////////////////////////////////////////////////////////////////
//
//  Script   : SRandom.cs
//  Info     : 随机数发生器(可用于同步随机数，避免平台自身随机性差异)
//  Author   : ls9512
//  E-mail   : ls9512@vip.qq.com
//
//  Copyright : Aya Game Studio 2020
//
/////////////////////////////////////////////////////////////////////////////
using System;

namespace Aya.Maths
{
    public class SRandom 
    {
        public static int Count = 0;

        public long Seed { get; protected set; }

        public SRandom(int seed)
        {
            Seed = seed;
        }

        public int Next()
        {
            Seed = Seed * 1103515245 + 12345;
            var result = (int)(Seed / 65536);
            return result;
        }

        public int Next(int max)
        {
            var result = Next() % max;
            return result;
        }

        public int Next(int min, int max)
        {
            if (min > max)
            {
                throw new ArgumentOutOfRangeException(nameof(min), $"'{min}' cannot be greater than {max}.");
            }

            var num = max - min;
            var result = Next(num) + min;
            return result;
        }
    }
}
