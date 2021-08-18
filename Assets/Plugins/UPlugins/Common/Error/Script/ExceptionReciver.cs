/////////////////////////////////////////////////////////////////////////////
//
//  Script   : ExceptionReciver.cs
//  Info     : 异常接收器，仅接收主线程上的未处理异常
//  Author   : ls9512
//  E-mail   : ls9512@vip.qq.com
//
//  Copyright : Aya Game Studio 2018
//
/////////////////////////////////////////////////////////////////////////////
using System;

namespace Aya.Common
{
    public static class ExceptionReciver
    {
        public static Action<object, Exception> OnUnhandledException = delegate { };

        static ExceptionReciver()
        {
            AppDomain.CurrentDomain.UnhandledException += (sender, args) =>
            {
                var exception = args.ExceptionObject as Exception;
                OnUnhandledException(sender, exception);
            };
        }
    }
}

