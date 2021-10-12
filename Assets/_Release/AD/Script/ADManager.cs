using System.Collections;
using System.Collections.Generic;
using Aya.Analysis;
using Aya.Async;
using UnityEngine;
using Aya.SDK;

namespace Aya.AD
{
    public class ADManager
    {
        public static ADChannelBase Instance { get; private set; }

        static ADManager()
        {
        }

        public static void Init()
        {
            SDKDebug.Log("AD", "Init.");
            CreateSdkInstance();
        }

        private static void CreateSdkInstance()
        {
#if UNITY_EDITOR
            SDKDebug.Log("AD", "Init Editor.");
            Instance = new ADEditor();
            Instance.Init();
            return;
#elif UNITY_ANDROID || UNITY_IOS
            SDKDebug.Log("AD", "Init AD Mobile.");

            Instance = new ADEditor();
#endif
            var setting = ADSetting.Load(Instance.GetType().Name);
            if (setting == null)
            {
                Debug.LogError("ADSetting not found!!!");
            }
            else
                Instance.Init(setting);
        }

    }
}
