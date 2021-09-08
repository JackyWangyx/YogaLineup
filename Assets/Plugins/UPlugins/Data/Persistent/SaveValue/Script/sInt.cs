/////////////////////////////////////////////////////////////////////////////
//
//  Script   : sInt.cs
//  Info     : 可存取数据类型 Int
//  Author   : ls9512
//  E-mail   : ls9512@vip.qq.com
//
//  Copyright : Aya Game Studio 2019
//
/////////////////////////////////////////////////////////////////////////////
using System;
using Aya.Security;

namespace Aya.Data.Persistent
{
    [Serializable]
    public class sInt : SaveValue<int>
    {
        public sInt(string key, int defaultValue = 0) : base(key, defaultValue)
        {
        }

        #region Override operator
        public static int operator +(sInt lhs, int rhs)
        {
            return lhs.Value + rhs;
        }

        public static int operator -(sInt lhs, int rhs)
        {
            return lhs.Value - rhs;
        }

        public static int operator *(sInt lhs, int rhs)
        {
            return lhs.Value = rhs;
        }

        public static int operator /(sInt lhs, int rhs)
        {
            return lhs.Value / rhs;
        }

        public static sInt operator ++(sInt lhs)
        {
            lhs.Value++;
            return lhs;
        }

        public static sInt operator --(sInt lhs)
        {
            lhs.Value--;
            return lhs;
        }

        public static bool operator ==(sInt lhs, sInt rhs)
        {
            return lhs.Value == rhs.Value;
        }

        public static bool operator !=(sInt lhs, sInt rhs)
        {
            return lhs.Value != rhs.Value;
        }

        public static implicit operator int(sInt obj)
        {
            return obj.Value;
        }

        public static implicit operator cInt(sInt obj)
        {
            cInt ret = obj.Value;
            return ret;
        }
        #endregion

        #region Override object

        public bool Equals(int obj)
        {
            return Value == obj;
        }

        public bool Equals(cInt obj)
        {
            return Value == obj;
        }

        public override bool Equals(object obj)
        {
            return this == (sInt)obj;
        }

        public override string ToString()
        {
            return Value.ToString();
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        #endregion
    }

}