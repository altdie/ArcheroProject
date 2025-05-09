using System;
using Project.Scripts.Players;
using UnityEngine.Advertisements;

namespace Project.Scripts.ADS
{
    public class RewardedAds : IUnityAdsLoadListener, IUnityAdsShowListener
    {
        public event Action OnAdWatched;
        private readonly string _androidAdUnitId = "Rewarded_Android";
        private readonly string _iOSAdUnitId = "Rewarded_iOS";
        private string _adUnitId = null;

        public void LoadAd()
        {
            var saveSystem = new PlayerPrefsSave();
            var playerData = saveSystem.Load();

            if (playerData.IsAdsRemoved)
            {
                OnAdWatched?.Invoke();
                return;
            }

            _adUnitId = _iOSAdUnitId;
            _adUnitId = _androidAdUnitId;
            Advertisement.Load(_adUnitId, this);
        }

        public void ShowAd()
        {
            var saveSystem = new PlayerPrefsSave();
            var playerData = saveSystem.Load();

            if (playerData.IsAdsRemoved)
            {
                return;
            }

            Advertisement.Show(_adUnitId, this);
        }

        public void OnUnityAdsShowComplete(string adUnitId, UnityAdsShowCompletionState showCompletionState)
        {
            if (adUnitId.Equals(_adUnitId) && showCompletionState.Equals(UnityAdsShowCompletionState.COMPLETED))
            {
                OnAdWatched?.Invoke();

                OnDestroy();
            }
        }

        public void OnUnityAdsAdLoaded(string adUnitId) { }
        public void OnUnityAdsFailedToLoad(string adUnitId, UnityAdsLoadError error, string message) { }
        public void OnUnityAdsShowFailure(string adUnitId, UnityAdsShowError error, string message) { }
        public void OnUnityAdsShowStart(string adUnitId) { }
        public void OnUnityAdsShowClick(string adUnitId) { }
        void OnDestroy() { }

        public void Initialize() { }
    }
}