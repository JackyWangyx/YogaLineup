/////////////////////////////////////////////////////////////////////////////
//
//  Script : TimeShareBehaviour.cs
//  Info   : TimeShare 的 MonoBehaviour 通用封装，简化使用
//  Author : ls9512
//  E-mail : ls9512@vip.qq.com
//
//  Copyright : Aya Game Studio 2018
//
/////////////////////////////////////////////////////////////////////////////
using UnityEngine;

namespace Aya.Timers
{
    public class TimeShareBehaviour : MonoBehaviour
    {
        /// <summary>
        /// 分组键
        /// </summary>
        public string Key = TimeShare.DefaultKey;
        /// <summary>
        /// 是否使用单一模式<para/>
        /// 单一模式 ：使用对象本身作为键，单一时间分组
        /// 非单一模式 ： 使用 Key 进行分组
        /// </summary>
        public bool SingleMode = false;

        /// <summary>
        /// 帧缩放
        /// </summary>
        public float DeltaTime => SingleMode ? TimeShare.GetDeltaTime(this) : TimeShare.GetDeltaTime(Key);

        /// <summary>
        /// 帧缩放(不受全局缩放影响)
        /// </summary>
        public float DeltaTimeWithoutGlobal => SingleMode ? TimeShare.GetDeltaTimeWithoutGlobal(this) : TimeShare.GetDeltaTimeWithoutGlobal(Key);

        /// <summary>
        /// 时间缩放
        /// </summary>
        public float TimeScale
        {
            get => SingleMode ? TimeShare.GetTimeScale(this) : TimeShare.GetTimeScale(Key);
            set
            {
                if (SingleMode)
                {
                    TimeShare.SetTimeScale(this, value);
                }
                else
                {
                    TimeShare.SetTimeScale(Key, value);
                }
            }
        }
    }
}

