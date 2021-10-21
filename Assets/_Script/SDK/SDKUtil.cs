using System;
using Aya.AD;
using Aya.Analysis;

public static class SDKUtil
{
    public static void Init()
    {
        AnalysisManager.Instance.Init();
        ADManager.Instance.Init();
    }

    public static bool IsRewardVideoReady(string key)
    {
        var result = ADManager.Instance.IsRewardedVideoReady();
        AnalysisManager.Instance.Event($"RewardVideo {key} is ready : {result}");
        return result;
    }

    public static void RewardVideo(string key, Action onSuccess = null, Action onFail = null)
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
    }
}
