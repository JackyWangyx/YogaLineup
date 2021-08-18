/////////////////////////////////////////////////////////////////////////////
//
//  Script   : sString.cs
//  Info     : 可存取数据类型 String
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
    public class sString : SaveValue<string>
    {
        public sString(string key, string defaultValue = "") : base(key, defaultValue)
        {
        }

        #region Override operator

        public static bool operator ==(sString lhs, sString rhs)
        {
            return lhs.Value == rhs.Value;
        }

        public static bool operator !=(sString lhs, sString rhs)
        {
            return lhs.Value != rhs.Value;
        }

        public static implicit operator string(sString obj)
        {
            return obj.Value;
        }

        #endregion

        #region Override object

        public bool Equals(string obj)
        {
            return Value == obj;
        }

        public override bool Equals(object obj)
        {
            return this == (sString)obj;
        }

        public override string ToString()
        {
            return Value;
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        #endregion
    }

}