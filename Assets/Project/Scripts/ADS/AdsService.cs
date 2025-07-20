using System;
using System.Diagnostics;
using Project.Scripts.Players;

namespace Project.Scripts.ADS
{
    public class AdsService
    {
        private readonly RewardedAds _rewardedAds;
        private readonly InterstitialAds _interstitialAds;
        private readonly PlayerPrefsSave _save;

        public AdsService(RewardedAds rewardedAds, InterstitialAds interstitialAds, PlayerPrefsSave save)
        {
            _rewardedAds = rewardedAds;
            _interstitialAds = interstitialAds;
            _save = save;
            _rewardedAds.Initialize(_save);
            _interstitialAds.Initialize(_save);
        }

        public void ShowRewardedAd(Action reward)
        {
            _rewardedAds.ShowAd(reward);
        }

        public void ShowInterstitialAd()
        {
            _interstitialAds.ShowAd();
        }

        public void LoadRewardedAd()
        {
            _rewardedAds.LoadAd();
        }

        public void LoadInterstitialAd()
        {
            _interstitialAds.LoadAd();
        }
    }
}
