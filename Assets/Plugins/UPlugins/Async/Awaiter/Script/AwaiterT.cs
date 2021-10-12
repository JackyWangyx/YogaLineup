/////////////////////////////////////////////////////////////////////////////
//
//  Script   : AwaiterT.cs
//  Info     : Awaiter<T> await 异步等待类
//  Author   : ls9512
//  E-mail   : ls9512@vip.qq.com
//
//  Copyright : Aya Game Studio 2019
//
/////////////////////////////////////////////////////////////////////////////
using System;

namespace Aya.Async
{
    public class Awaiter<T> : IAwaiter<T>
    {
        public virtual bool IsCompleted { get; private set; } = false;
        public Action Action { get; private set; }
        public T Result { get; private set; }

        public virtual void OnCompleted(Action continuation)
        {
            Action = continuation;
        }

        public void UnsafeOnCompleted(Action continuation)
        {
            Action = continuation;
        }

        public virtual Awaiter<T> GetAwaiter() => this;
        public virtual T GetResult() => Result;

        public virtual void Complete(T result)
        {
            IsCompleted = true;
            Result = result;
            Action?.Invoke();
        }
    }
}
