﻿/////////////////////////////////////////////////////////////////////////////
//
//  Script   : IAwaiterT.cs
//  Info     : IAwaiter<T> 接口定义
//  Author   : ls9512
//  E-mail   : ls9512@vip.qq.com
//
//  Copyright : Aya Game Studio 2019
//
/////////////////////////////////////////////////////////////////////////////
using System.Runtime.CompilerServices;

namespace Aya.Async
{
    public interface IAwaiter<T> : ICriticalNotifyCompletion
    {
        void Complete(T result);
        T GetResult();
    }

}