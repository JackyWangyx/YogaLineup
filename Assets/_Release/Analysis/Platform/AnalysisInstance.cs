//#define GoogleAnalytic
#if FB
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Aya.Util;
using Facebook.Unity;
using Firebase.Analytics;
using UnityEngine;

namespace Aya.Analysis
{
    public class AnalysisInstance : AnalysisBase
    {
        //        protected GoogleAnalyticsV4 GoogleAnalytics;

        public override void Init()
        {
            base.Init();

#if !UNITY_EDITOR
            FB.Init(
                () =>
                {
                    if (FB.IsInitialized)
                    {
                        AnalysisDebug.Log("Login : " + FB.IsLoggedIn + "\t\tInit : " + FB.IsInitialized);
                        //FB.ActivateApp();
                    }
                    else
                    {
                        AnalysisDebug.Log("Failed to Initialize the Facebook SDK");
                    }
                },
                isGameShown =>
                {
                    if (!isGameShown)
                    {
                        Time.timeScale = 0;
                    }
                    else
                    {
                        Time.timeScale = 1;
                    }
                });

            AppsFlyer.setAppsFlyerKey("qKnMipNNDACuKe2SKdNFai");
#if UNITY_IOS
            AppsFlyer.setAppID ("1507350875");
            AppsFlyer.trackAppLaunch ();
#elif UNITY_ANDROID
            AppsFlyer.setAppID("com.QuickLoad.MergeBallBlast");
            AppsFlyer.init("qKnMipNNDACuKe2SKdNFai", "AppsFlyerTrackerCallbacks");
#endif

#endif
        }

        public override void MissionStart(string mission, MissionType type)
        {
            Event("MissionStart", "LevelName", mission);
        }

        public override void MissionCompleted(string mission)
        {
            Event("MissionCompleted", "LevelName", mission);
        }

#if GoogleAnalytic
		public override void Event(string category, string action, string lable, int value)
        {
            AnalysisDebug.Log("Event " + category + "\t" + action + "\t" + lable + "\t" + value);
            GoogleAnalytics.LogEvent(new EventHitBuilder()
                .SetEventCategory(category)
                .SetEventAction(action)
                .SetEventLabel(lable)
                .SetEventValue(value));
        }
#endif

        public override void Event(string eventID, Dictionary<string, object> args = null)
        {
            if (AnalysisDebug.IsDebug)
            {
                string str = "Event :" + eventID;
                if (args != null)
                    foreach (var Dic in args)
                    {
                        str += (" + " + Dic.Key + " : " + Dic.Value);
                    }
                AnalysisDebug.Log(str);
            }  

#if UNITY_EDITOR
            // return;
#endif

            if (FB.IsInitialized)
            {
                // FB App Event
                FB.LogAppEvent(
                    eventID,
                    1,
                    args);

                // 激励视频
                if (eventID == "RewardVideo" && args.ContainsKey("State") && args["State"].ToString() == "Success")
                {
                    var parameters = args;
                    parameters[AppEventParameterName.ContentType] = "";
                    parameters[AppEventParameterName.Description] = "";
                    parameters[AppEventParameterName.ContentID] = "";
                    parameters[AppEventParameterName.Currency] = "";
                    FB.LogAppEvent(AppEventName.ViewedContent, 1, parameters);
                }

                // 内购
                if (eventID == "Purchase")
                {
                    // var parameters = args;
                    // FB.LogPurchase();
                }

                // 订阅
                if (eventID == "Member")
                {
                    // var parameters = args;
                }
            }
            if (FireBaseSDKManager.Instance == null || FireBaseSDKManager.Instance.app == null) return;
            var argsArray = new List<Parameter>();
            var argDic = new Dictionary<string, string>();
            if (args != null)
            {
                foreach (var key in args.Keys)
                {
                    var p = new Parameter(key, args[key].ToString());
                    argsArray.Add(p);
                    argDic.Add(key, args[key].ToString());
                }
            }
            FirebaseAnalytics.LogEvent(eventID, argsArray.ToArray());

            if (eventID == "Purchase" && args.ContainsKey("State") && args["State"].ToString() == "Success")
            {
                AppsFlyer.trackRichEvent(AFInAppEvents.LEVEL_ACHIEVED, new Dictionary<string, string>(){
                    {AFInAppEvents.CONTENT_ID, args["GoodsID"].ToString()},
                    {AFInAppEvents.CONTENT_TYPE, ""},
                    {AFInAppEvents.REVENUE, ""},
                    {AFInAppEvents.CURRENCY, "USD"}
                });
            }
            else
            {
                AppsFlyer.trackRichEvent(eventID, argDic);
            }
        }

    }
}
#endif