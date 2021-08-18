/////////////////////////////////////////////////////////////////////////////
//
//  Script   : ErrorCodeEnum.cs
//  Info     : 错误码类型定义
//  Author   : ls9512
//  E-mail   : ls9512@vip.qq.com
//
//  Copyright : Aya Game Studio 2016
//
/////////////////////////////////////////////////////////////////////////////

namespace Aya.Common {
	public enum ErrorCodeEnum
	{
		/// <summary>
		/// 成功
		/// </summary>
		Success = 0,

		/// <summary>
		/// 提示
		/// </summary>
		Tip = 1,

		/// <summary>
		/// 警告
		/// </summary>
		Warning = 2,

		/// <summary>
		/// 重试
		/// </summary>
		Retry = 3,

		/// <summary>
		/// 退出
		/// </summary>
		Exit = 4
	}
}