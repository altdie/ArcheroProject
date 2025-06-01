using System;

namespace Assets.Project.Scripts.ADS
{
    public interface IAds
    {
        void ShowRewardedAd(Action reward);
        void ShowInterstitialAd();
        void LoadRewardedAd();
        void LoadInterstitialAd();
    }
}
