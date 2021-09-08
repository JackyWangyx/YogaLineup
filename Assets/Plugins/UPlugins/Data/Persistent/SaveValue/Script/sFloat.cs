/////////////////////////////////////////////////////////////////////////////
//
//  Script   : sFloat.cs
//  Info     : 可存取数据类型 Float
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
    public class sFloat : SaveValue<float>
    {
        public sFloat(string key, float defaultValue = 0f) : base(key, defaultValue)
        {
        }

        #region Override operator
        public static float operator +(sFloat lhs, float rhs)
        {
            return lhs.Value + rhs;
        }

        public static float operator -(sFloat lhs, float rhs)
        {
            return lhs.Value - rhs;
        }

        public static float operator *(sFloat lhs, float rhs)
        {
            return lhs.Value * rhs;
        }

        public static float operator /(sFloat lhs, float rhs)
        {
            return lhs.Value / rhs;
        }

        public static sFloat operator ++(sFloat lhs)
        {
            lhs.Value++;
            return lhs;
        }

        public static sFloat operator --(sFloat lhs)
        {
            lhs.Value--;
            return lhs;
        }

        public static bool operator ==(sFloat lhs, sFloat rhs)
        {
            return lhs.Value == rhs.Value;
        }

        public static bool operator !=(sFloat lhs, sFloat rhs)
        {
            return lhs.Value != rhs.Value;
        }


        public static implicit operator float(sFloat obj)
        {
            return obj.Value;
        }

        public static implicit operator cFloat(sFloat obj)
        {
            cFloat ret = obj.Value;
            return ret;
        }

        #endregion

        #region Override object

        public bool Equals(float obj)
        {
            return Value == obj;
        }

        public bool Equals(cFloat obj)
        {
            return Value == obj;
        }

        public override bool Equals(object obj)
        {
            return this == (sFloat)obj;
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