/////////////////////////////////////////////////////////////////////////////
//
//  Script   : Awaiter.cs
//  Info     : Awaiter await 异步等待类
//  Author   : ls9512
//  E-mail   : ls9512@vip.qq.com
//
//  Copyright : Aya Game Studio 2019
//
/////////////////////////////////////////////////////////////////////////////
using System;

namespace Aya.Async
{
    public class Awaiter : IAwaiter
    {
        public virtual bool IsCompleted { get; private set; } = false;
        public Action Action { get; private set; }

        public virtual void OnCompleted(Action continuation)
        {
            Action = continuation;
        }

        public void UnsafeOnCompleted(Action continuation)
        {
            Action = continuation;
        }

        public virtual Awaiter GetAwaiter() => this;
        public virtual void GetResult() { }

        public virtual void Complete()
        {
            IsCompleted = true;
            Action?.Invoke();
        }
    }
}
