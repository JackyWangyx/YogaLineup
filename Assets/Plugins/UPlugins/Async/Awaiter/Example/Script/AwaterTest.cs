using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using Aya.Async;
using Aya.Extension;

namespace Aya.Sample
{
    public class AwaterTest : MonoBehaviour
    {
        public TestAsync1 TestAsync1 = new TestAsync1();
        public TestAsync2 TestAsync2 = new TestAsync2();
        public TestAsync3 TestAsync3 = new TestAsync3();
        public TestAsync4 TestAsync4 = new TestAsync4();

        public void Start()
        {
            Test1();
            Test2();
            Test3();
            Test4();

            this.ExecuteDelay(() =>
            {
                TestAsync1.Complete();
                TestAsync2.Complete();
                TestAsync3.Complete(3);
                TestAsync4.Complete(4);
            }, 5f);
        }

        public async void Test1()
        {
            Debug.Log("Start 1");        
            await TestAsync1;
            Debug.Log("End 1");
        }

        public async void Test2()
        {
            Debug.Log("Start 2");
            await TestAsync2;
            Debug.Log("End 2");
        }

        public async void Test3()
        {
            Debug.Log("Start 3");
            await TestAsync3;
            Debug.Log("End 3 - result:" + TestAsync3.Result);
        }

        public async void Test4()
        {
            Debug.Log("Start 4");
            await TestAsync4;
            Debug.Log("End 4 - result:" + TestAsync4.Result);
        }
    }

    // 使用方法1
    public class TestAsync1
    {
        public Awaiter Awaiter { get; set; }
        public Awaiter GetAwaiter() => Awaiter;

        public TestAsync1()
        {
            Awaiter = new Awaiter();
        }

        public void Complete()
        {
            Awaiter.Complete();
        }
    }

    // 使用方法2
    public class TestAsync2 : Awaiter
    {
    }

    // 使用方法3
    public class TestAsync3
    {
        public Awaiter<int> Awaiter { get; set; }
        public Awaiter<int> GetAwaiter() => Awaiter;

        public TestAsync3()
        {
            Awaiter = new Awaiter<int>();
        }

        public void Complete(int result)
        {
            Awaiter.Complete(result);
        }

        public int Result => Awaiter.GetResult();
    }

    // 使用方法4
    public class TestAsync4 : Awaiter<int>
    {
    }
}
