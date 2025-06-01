using System;
using Project.Scripts.Players;
using UnityEngine.Advertisements;

namespace Project.Scripts.ADS
{
    public class RewardedAds : IUnityAdsLoadListener, IUnityAdsShowListener
    {
        private Action _onAdWatchedCallback;
        private readonly string _androidAdUnitId = "Rewarded_Android";
        private readonly string _iOSAdUnitId = "Rewarded_iOS";
        private string _adUnitId = null;

        public void LoadAd()
        {
            var saveSystem = new PlayerPrefsSave();
            var playerData = saveSystem.Load();

            if (playerData.IsAdsRemoved)
            {
                return;
            }

            _adUnitId = _iOSAdUnitId;
            _adUnitId = _androidAdUnitId;
            Advertisement.Load(_adUnitId, this);
        }

        public void ShowAd(Action onAdWatchedCallback)
        {
            var saveSystem = new PlayerPrefsSave();
            var playerData = saveSystem.Load();

            if (playerData.IsAdsRemoved)
            {
                return;
            }

            _onAdWatchedCallback += onAdWatchedCallback;

            Advertisement.Show(_adUnitId, this);
        }

        public void OnUnityAdsShowComplete(string adUnitId, UnityAdsShowCompletionState showCompletionState)
        {
            if (adUnitId.Equals(_adUnitId) && showCompletionState.Equals(UnityAdsShowCompletionState.COMPLETED))
            {
                _onAdWatchedCallback?.Invoke();
                _onAdWatchedCallback = null;

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