using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Aya.AD
{
    public class ADEditor : ADChannelBase<ADEditorLocationBanner, ADEditorLocationInterstitial, ADEditorLocationRewardedVideo>
    {
        public override void Init(params object[] args) { }

        public override void ShowBanner() { }

        public override bool IsInterstitialReady() => true;

        public override void ShowInterstitial(Action<bool> onDone = null) => onDone?.Invoke(true);

        public override bool IsRewardedVideoReady() => true;

        public override void ShowRewardedVideo(Action<bool> onDone = null) => onDone?.Invoke(true);

    }

    #region AD Editor Location

    public class ADEditorLocationBanner : ADLocationBase<ADEditorSourceBanner>
    {
    }

    public class ADEditorLocationInterstitial : ADLocationBase<ADEditorSourceInterstitial>
    {
    }

    public class ADEditorLocationRewardedVideo : ADLocationBase<ADEditorSourceRewardedVideo>
    {
    }

    #endregion

    #region AD Editor Source

    public class ADEditorSourceBanner : ADEditorSourceBase
    {
        public override ADLocationType Type
        {
            get { return ADLocationType.Banner; }
        }
    }

    public class ADEditorSourceInterstitial : ADEditorSourceBase
    {
        public override ADLocationType Type
        {
            get { return ADLocationType.Interstitial; }
        }
    }

    public class ADEditorSourceRewardedVideo : ADEditorSourceBase
    {
        public override ADLocationType Type
        {
            get { return ADLocationType.RewardedVideo; }
        }
    }

    public class ADEditorSourceBase : ADSourceBase
    {
        public override ADLocationType Type
        {
            get { return ADLocationType.None; }
        }

        public override bool IsReady
        {
            get { return true; }
        }

        public override void Init(params object[] args)
        {
            IsInited = true;
            OnInited(true);
        }

        public override void Load(Action<bool> onDone = null)
        {
            OnLoaded(true);
            if (onDone != null) onDone(true);
        }

        public override void Show(Action<bool> onDone = null)
        {
            OnShowed();
            if (onDone != null) onDone(true);
            OnResult(true);
        }

        public override void Close()
        {
            OnCloseed();
        }
    }

    #endregion
}

