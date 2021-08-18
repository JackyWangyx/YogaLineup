/////////////////////////////////////////////////////////////////////////////
//
//  Script : TimeShare.cs
//  Info   : 共享时间，可以按照键值将游戏对象分组并设置不同的时间缩放
//  Author : ls9512
//  E-mail : ls9512@vip.qq.com
//
//  Info   : 1.建议创建一个类和静态变量缓存某一分组时间的读取，简化编码，参考 GlobalTimeScale 的实现
//         : 2.注意字典的性能
//
//  Copyright : Aya Game Studio 2018
//
/////////////////////////////////////////////////////////////////////////////
using System.Collections.Generic;
using UnityEngine;

namespace Aya.Timers
{
    public static class TimeShare
    {
        #region Public
        /// <summary>
        /// 全局时间缩放，会乘到所有分组上
        /// </summary>
        public static float GlobalTimeScale
        {
            get => GetValue(KeyScaleDic, DefaultKey, DefaultValue);
            set => TryAdd(KeyScaleDic, DefaultKey, value);
        }
        /// <summary>
        /// 引擎时间
        /// </summary>
        public static float UnityDeltaTime => Time.deltaTime;

        #endregion

        #region Private
        /// <summary>
        /// 全局时间缩放分组键
        /// </summary>
        internal static string DefaultKey = "Global";
        /// <summary>
        /// 默认时间缩放值
        /// </summary>
        internal static float DefaultValue = 1f;
        /// <summary>
        /// 对象 - 分组键 字典
        /// </summary>
        private static readonly Dictionary<object, string> ObjKeyDic = new Dictionary<object, string>();
        /// <summary>
        /// 分组键 - 时间缩放值 字典
        /// </summary>
        private static readonly Dictionary<string, float> KeyScaleDic = new Dictionary<string, float>();
        #endregion

        #region DeltaTime
        /// <summary>
        /// 获取帧时间
        /// </summary>
        /// <param name="key">分组键</param>
        /// <returns>时间缩放值(默认值为1)</returns>
        public static float GetDeltaTime(string key)
        {
            var scale = GetValue(KeyScaleDic, key, DefaultValue);
            return scale * GlobalTimeScale * UnityDeltaTime;
        }

        /// <summary>
        /// 获取帧时间(不受全局缩放影响)
        /// </summary>
        /// <param name="key">分组键</param>
        /// <returns>时间缩放值(默认值为1)</returns>
        public static float GetDeltaTimeWithoutGlobal(string key)
        {
            var scale = GetValue(KeyScaleDic, key, DefaultValue);
            return scale * UnityDeltaTime;
        }

        /// <summary>
        /// 获取帧时间
        /// </summary>
        /// <param name="obj">对象</param>
        /// <returns>时间缩放值(默认值为1)</returns>
        public static float GetDeltaTime(object obj)
        {
            var key = GetValue(ObjKeyDic, obj, DefaultKey);
            var scale = GetValue(KeyScaleDic, key, DefaultValue);
            return scale * GlobalTimeScale * UnityDeltaTime;
        }

        /// <summary>
        /// 获取帧时间(不受全局缩放影响)
        /// </summary>
        /// <param name="obj">对象</param>
        /// <returns>时间缩放值(默认值为1)</returns>
        public static float GetDeltaTimeWithoutGlobal(object obj)
        {
            var key = GetValue(ObjKeyDic, obj, DefaultKey);
            var scale = GetValue(KeyScaleDic, key, DefaultValue);
            return scale * UnityDeltaTime;
        }
        #endregion

        #region TimeScale
        /// <summary>
        /// 获取时间缩放
        /// </summary>
        /// <param name="key">分组键</param>
        /// <returns>时间缩放值(默认值为1)</returns>
        public static float GetTimeScale(string key)
        {
            var scale = GetValue(KeyScaleDic, key, DefaultValue);
            return scale;
        }

        /// <summary>
        /// 获取时间缩放
        /// </summary>
        /// <param name="obj">对象</param>
        /// <returns>时间缩放值(默认值为1)</returns>
        public static float GetTimeScale(object obj)
        {
            var key = GetValue(ObjKeyDic, obj, DefaultKey);
            var scale = GetValue(KeyScaleDic, key, DefaultValue);
            return scale;
        }

        /// <summary>
        /// 设置时间缩放
        /// </summary>
        /// <param name="key">分组键</param>
        /// <param name="timeScale">时间缩放</param>
        public static void SetTimeScale(string key, float timeScale)
        {
            TryAdd(KeyScaleDic, key, timeScale);
        }

        /// <summary>
        /// 设置时间缩放
        /// </summary>
        /// <param name="obj">对象</param>
        /// <param name="timeScale">时间缩放</param>
        public static void SetTimeScale(object obj, float timeScale)
        {
            var key = GetValue(ObjKeyDic, obj, DefaultKey);
            TryAdd(KeyScaleDic, key, timeScale);
        } 
        #endregion

        #region Private Method

        private static TValue GetValue<TKey, TValue>(IDictionary<TKey, TValue> dic, TKey key, TValue defaultValue = default(TValue))
        {
            if (key == null) return defaultValue;
            var ret = dic[key];
            return ret;
        }

        private static void TryAdd<TKey, TValue>(IDictionary<TKey, TValue> dic, TKey key, TValue value)
        {
            dic[key] = value;
        } 

        #endregion
    }
}
