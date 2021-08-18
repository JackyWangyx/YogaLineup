/////////////////////////////////////////////////////////////////////////////
//
//  Script   : ErrorCodeDefine.cs
//  Info     : 错误码定义
//  Author   : ls9512
//  E-mail   : ls9512@vip.qq.com
//
//  Copyright : Aya Game Studio 2016
//
/////////////////////////////////////////////////////////////////////////////

namespace Aya.Common {
	public sealed partial class ErrorCode {
		public static ErrorCode Success = new ErrorCode(0, ErrorCodeEnum.Success, "Success");
		public static ErrorCode InternalError = new ErrorCode(500, ErrorCodeEnum.Warning, "Internal Error");
		public static ErrorCode LogicError = new ErrorCode(2000, ErrorCodeEnum.Warning, "Logic Error");
		public static ErrorCode NullPointer = new ErrorCode(10000, ErrorCodeEnum.Warning, "Null Pointer");
	}
}

