/////////////////////////////////////////////////////////////////////////////
//
//  Script   : Error.cs
//  Info     : 错误处理类
//  Author   : ls9512
//  E-mail   : ls9512@vip.qq.com
//
//  Copyright : Aya Game Studio 2017
//
/////////////////////////////////////////////////////////////////////////////
using System;
using System.Collections.Generic;

namespace Aya.Common
{
	public static class Error
	{
		/// <summary>
		/// 错误回调
		/// </summary>
		public static Action<Exception> OnErrorCallback = delegate { };

		#region Run
		/// <summary>
		/// 执行操作，并处理指定异常
		/// </summary>
		/// <typeparam name="T">异常类型</typeparam>
		/// <param name="action">操作</param>
		/// <param name="onError">异常处理</param>
		public static void Run<T>(Action action, Action<T> onError = null) where T : Exception, new() {
			try
			{
				action();
			} 
			catch (T e)
			{
				OnError(e);
				if (onError != null)
				{
					onError(e);
				}
			}
		}

		/// <summary>
		/// 执行操作，并处理指定异常
		/// </summary>
		/// <param name="action">操作</param>
		/// <param name="onError">异常处理</param>
		public static void Run(Action action, Action<Exception> onError = null) {
			try
			{
				action();
			} catch (Exception e)
			{
				OnError(e);
				if (onError != null)
				{
					onError(e);
				}
			}
		}
		#endregion

		#region Check

		/// <summary>
		/// 检查，不成立则处理异常
		/// </summary>
		/// <typeparam name="T">异常</typeparam>
		/// <param name="check">检查表达式</param>
		public static void Check<T>(bool check) where T : Exception, new()
		{
			if (!check)
			{
				OnError(new T());
			}
		}

		/// <summary>
		/// 检查，不成立则处理异常
		/// </summary>
		/// <param name="check">检查表达式</param>
		/// <param name="error">异常描述</param>
		public static void Check(bool check, string error)
		{
			if (!check)
			{
				OnError(new Exception(error));
			}
		}

		#endregion

		#region CheckNotNullOrEmpty

		/// <summary>
		/// 检查对象非空，如空则抛出异常
		/// </summary>
		/// <param name="obj">对象</param>
		/// <param name="error">异常信息</param>
		public static void CheckNotNullOrEmpty(object obj, string error = "") {
			if (obj == null)
			{
				OnError(new ArgumentNullException(error));
			}
		}

		/// <summary>
		/// 检查字符串非空，如空则抛出异常
		/// </summary>
		/// <param name="str">字符串</param>
		/// <param name="error">异常信息</param>
		public static void CheckNotNullOrEmpty(string str, string error = "") {
			if (string.IsNullOrEmpty(str))
			{
				OnError(new ArgumentNullException(error));
			}
		}

		/// <summary>
		/// 检查集合非空，如空则抛出异常
		/// </summary>
		/// <typeparam name="T">类型</typeparam>
		/// <param name="list">集合</param>
		/// <param name="error">异常信息</param>
		public static void CheckNotNullOrEmpty<T>(IList<T> list, string error = "") {
			if (list == null || list.Count == 0)
			{
				OnError(new ArgumentNullException(error));
			}
		}

		/// <summary>
		/// 检查数组非空，如空则抛出异常
		/// </summary>
		/// <typeparam name="T">类型</typeparam>
		/// <param name="list">集合</param>
		/// <param name="error">异常信息</param>
		public static void CheckNotNullOrEmpty<T>(T[] list, string error = "") {
			if (list == null || list.Length == 0)
			{
				OnError(new ArgumentNullException(error));
			}
		}
		#endregion

		#region CheckNullOrEmpty

		/// <summary>
		/// 检查对象空，如非空则抛出异常
		/// </summary>
		/// <param name="obj">对象</param>
		/// <param name="error">异常信息</param>
		public static void CheckNullOrEmpty(object obj, string error = "") {
			if (obj != null)
			{
				OnError(new ArgumentNullException(error));
			}
		}

		/// <summary>
		/// 检查字符串空，如非空则抛出异常
		/// </summary>
		/// <param name="str">字符串</param>
		/// <param name="error">异常信息</param>
		public static void CheckNullOrEmpty(string str, string error = "") {
			if (!string.IsNullOrEmpty(str))
			{
				OnError(new ArgumentNullException(error));
			}
		}

		/// <summary>
		/// 检查集合空，如非空则抛出异常
		/// </summary>
		/// <typeparam name="T">类型</typeparam>
		/// <param name="list">集合</param>
		/// <param name="error">异常信息</param>
		public static void CheckNullOrEmpty<T>(IList<T> list, string error = "") {
			if (list != null && list.Count > 0)
			{
				OnError(new ArgumentNullException(error));
			}
		}

		/// <summary>
		/// 检查数组空，如非空则抛出异常
		/// </summary>
		/// <typeparam name="T">类型</typeparam>
		/// <param name="list">集合</param>
		/// <param name="error">异常信息</param>
		public static void CheckNullOrEmpty<T>(T[] list, string error = "") {
			if (list != null && list.Length > 0)
			{
				OnError(new ArgumentNullException(error));
			}
		}
		#endregion

		#region OnError

		/// <summary>
		/// 发生错误
		/// </summary>
		/// <param name="error">错误内容</param>
		public static void OnError(string error)
		{
			var ec = new ErrorCode(ErrorCode.InternalError.Code, ErrorCode.InternalError.Enum, ErrorCode.InternalError.Info,
				error);
			OnError(ec);
		}

		/// <summary>
		/// 发生错误
		/// </summary>
		/// <param name="exception">异常</param>
		public static void OnError(Exception exception)
		{
			var ec = new ErrorCode(ErrorCode.InternalError.Code, ErrorCode.InternalError.Enum, ErrorCode.InternalError.Info,
				exception.ToString());
			OnError(ec);
		}

		/// <summary>
		/// 发生错误
		/// </summary>
		/// <param name="errorCode">错误码</param>
		/// <param name="exception">异常</param>
		public static void OnError(ErrorCode errorCode, Exception exception)
		{
			// 错误处理

			// 抛出异常
			throw new Exception(errorCode.Desc, exception);
		}

		/// <summary>
		/// 发生错误
		/// </summary>
		/// <param name="errorCode">错误码</param>
		public static void OnError(ErrorCode errorCode)
		{
			// 错误处理
			// UnityEngine.Debug.LogError(errorCode.ToString());
			// 抛出异常
			throw new Exception(errorCode.ToString());
		}

		#endregion
	}
}

