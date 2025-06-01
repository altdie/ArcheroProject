using System;
using Assets.Project.Scripts.ADS;

namespace Project.Scripts.ADS
{
    public class AdsService : IAds // простой класс, бесполезный интерфейс который я не понимаю как использовать. Класс так же не поинмаю как использовать
    {
        private readonly RewardedAds _rewardedAds;
        private readonly InterstitialAds _interstitialAds;

        public AdsService(RewardedAds rewardedAds, InterstitialAds interstitialAds)
        {
            _rewardedAds = rewardedAds;
            _interstitialAds = interstitialAds;

            _rewardedAds.Initialize();
            _interstitialAds.Initialize();
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