/////////////////////////////////////////////////////////////////////////////
//
//  Script   : ErrorCode.cs
//  Info     : 错误码封装
//  Author   : ls9512
//  E-mail   : ls9512@vip.qq.com
//
//  Copyright : Aya Game Studio 2016
//
/////////////////////////////////////////////////////////////////////////////
using System;

namespace Aya.Common {
	[Serializable]
	public sealed partial class ErrorCode
	{
		/// <summary>
		/// 错误号
		/// </summary>
		public int Code { get; private set; }

		/// <summary>
		/// 错误类型
		/// </summary>
		public ErrorCodeEnum Enum { get; private set; }

		/// <summary>
		/// 错误信息
		/// </summary>
		public string Info { get; private set; }

		/// <summary>
		/// 错误描述
		/// </summary>
		public string Desc { get; private set; }

		/// <summary>
		/// 构造方法
		/// </summary>
		/// <param name="code">错误码</param>
		/// <param name="enum">错误类型</param>
		/// <param name="info">错误信息</param>
		/// <param name="desc">错误描述</param>
		public ErrorCode(int code, ErrorCodeEnum @enum, string info, string desc = null)
		{
			Code = code;
			Enum = @enum;
			Info = info ?? "";
			Desc = desc ?? "";
		}

		/// <summary>
		/// int 重载
		/// </summary>
		/// <param name="errorCode">错误码</param>
		public static implicit operator int(ErrorCode errorCode)
		{
			return errorCode != null ? errorCode.Code : -1;
		}

		/// <summary>
		/// bool 重载
		/// </summary>
		/// <param name="errorCode">错误码</param>
		public static implicit operator bool(ErrorCode errorCode)
		{
			return errorCode != null && errorCode.Code == Success.Code;
		}

		/// <summary>
		/// == 重载
		/// </summary>
		/// <param name="lhs">错误码1</param>
		/// <param name="rhs">错误码2</param>
		/// <returns>是否相等</returns>
		public static bool operator ==(ErrorCode lhs, ErrorCode rhs)
		{
			return Equals(lhs, rhs);
		}

		/// <summary>
		/// != 重载
		/// </summary>
		/// <param name="lhs">错误码1</param>
		/// <param name="rhs">错误码2</param>
		/// <returns>是否相等</returns>
		public static bool operator !=(ErrorCode lhs, ErrorCode rhs)
		{
			return !(lhs == rhs);
		}

		/// <summary>
		/// 比较
		/// </summary>
		/// <param name="obj">错误码</param>
		/// <returns>结果</returns>
		public bool Equals(ErrorCode obj)
		{
			return Equals(this, obj);
		}

		/// <summary>
		/// 比较
		/// </summary>
		/// <param name="obj">对象</param>
		/// <returns>结果</returns>
		public override bool Equals(object obj)
		{
			if (ReferenceEquals(null, obj)) return false;
			if (ReferenceEquals(this, obj)) return true;
			return obj is ErrorCode && Equals((ErrorCode) obj);
		}

		/// <summary>
		/// 获取哈希值
		/// </summary>
		/// <returns>结果</returns>
		public override int GetHashCode()
		{
			return Code;
		}

		/// <summary>
		/// 转换为字符串
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
			return Code + "\t" + Info + "\t" + Desc;
		}
	}
}
