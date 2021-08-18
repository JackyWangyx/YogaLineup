/////////////////////////////////////////////////////////////////////////////
//
//  Script   : sBool.cs
//  Info     : 可存取数据类型 Bool
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
    public class sBool : SaveValue<bool>
    {
        public sBool(string key, bool defaultValue = false) : base(key, defaultValue)
        {
        }

        #region Override operator

        public static implicit operator bool(sBool obj)
        {
            return obj.Value;
        }

        public static bool operator ==(sBool lhs, sBool rhs)
        {
            return lhs.Value == rhs.Value;
        }

        public static bool operator !=(sBool lhs, sBool rhs)
        {
            return lhs.Value != rhs.Value;
        }

        public static implicit operator cBool(sBool obj)
        {
            cBool ret = obj.Value;
            return ret;
        }

        #endregion

        #region Override object

        public bool Equals(bool obj)
        {
            return Value == obj;
        }

        public bool Equals(cBool obj)
        {
            return Value == obj;
        }

        public override bool Equals(object obj)
        {
            return this == (sBool)obj;
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