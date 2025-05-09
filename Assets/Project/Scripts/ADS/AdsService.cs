using Assets.Project.Scripts.ADS;

namespace Project.Scripts.ADS
{
    public class AdsService : IAds
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

        public void ShowRewardedAd()
        {
            _rewardedAds.ShowAd();
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
