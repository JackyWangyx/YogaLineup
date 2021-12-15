using System;
using Aya.AD;
using Aya.Analysis;
#if UNITY_IOS
using Unity.Advertisement.IosSupport;
#endif

public static class SDKUtil
{
    public static void Init()
    {
        RequestTrackingPermission();
        AnalysisManager.Instance.Init();
        ADManager.Init();
         ADManager.Instance.ShowBanner();
    }

    public static void RequestTrackingPermission()
    {
#if UNITY_IOS
        if (ATTrackingStatusBinding.GetAuthorizationTrackingStatus() == ATTrackingStatusBinding.AuthorizationTrackingStatus.NOT_DETERMINED)
        {
            ATTrackingStatusBinding.RequestAuthorizationTracking();
        }
#endif
    }

    public static bool IsRewardVideoReady(string key)
    {
        var result = ADManager.Instance.IsRewardedVideoReady();
        AnalysisManager.Instance.Event($"RewardVideo {key} is ready : {result}");
        return result;
    }

    public static void RewardVideo(string key, Action onSuccess = null, Action onFail = null, Action onNoAds = null)
    {
        if (ADManager.Instance.IsRewardedVideoReady())
        {
            AnalysisManager.Instance.Event($"RewardVideo {key} Start");
            ADManager.Instance.ShowRewardedVideo(result =>
            {
                if (result)
                {
                    AnalysisManager.Instance.Event($"RewardVideo {key} Success");
                    onSuccess?.Invoke();
                }
                else
                {
                    AnalysisManager.Instance.Event($"RewardVideo {key} Fail");
                    onFail?.Invoke();
                }
            });
        }
        else
        {
            onNoAds?.Invoke();
        }
    }
}
