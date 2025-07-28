using System;
using Project.Scripts.Players;
using Zenject;

namespace Project.Scripts.ADS
{
    public class AdsService : IInitializable
    {
        private readonly RewardedAds _rewardedAds;
        private readonly InterstitialAds _interstitialAds;
        private readonly PlayerPrefsSave _save;

        public AdsService(RewardedAds rewardedAds, InterstitialAds interstitialAds, PlayerPrefsSave save)
        {
            _rewardedAds = rewardedAds;
            _interstitialAds = interstitialAds;
            _save = save;
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

        public void Initialize()
        {
            _rewardedAds.Initialize(_save);
            _interstitialAds.Initialize(_save);
        }
    }
}
