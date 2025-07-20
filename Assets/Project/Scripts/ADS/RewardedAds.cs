using System;
using Firebase.Analytics;
using Project.Scripts.Players;
using UnityEngine;
using UnityEngine.Advertisements;

namespace Project.Scripts.ADS
{
    public class RewardedAds : IUnityAdsLoadListener, IUnityAdsShowListener
    {
        private Action _onAdWatchedCallback;
        private readonly string _androidAdUnitId = "Rewarded_Android";
        private readonly string _iOSAdUnitId = "Rewarded_iOS";
        private string _adUnitId;
        private PlayerPrefsSave _save;

        public void Initialize(PlayerPrefsSave save)
        {
            _adUnitId = (Application.platform == RuntimePlatform.IPhonePlayer)
                ? _iOSAdUnitId
                : _androidAdUnitId;

            _save = save;
        }

        public void LoadAd()
        {
            var data = _save.Load();
            Debug.Log($"[RewardedAds] IsAdsRemoved: {data.IsAdsRemoved}");

            if (data.IsAdsRemoved)
            {
                Debug.Log("[RewardedAds] Ads removed, simulating reward.");
                _onAdWatchedCallback?.Invoke();
                return;
            }

            Advertisement.Load(_adUnitId, this);
        }

        public void ShowAd(Action onAdWatchedCallback)
        {
            var data = _save.Load();

            if (data.IsAdsRemoved)
            {
                Debug.Log("[RewardedAds] Ads removed, ad skipped.");
                onAdWatchedCallback?.Invoke();
                return;
            }

            _onAdWatchedCallback += onAdWatchedCallback;
            Advertisement.Show(_adUnitId, this);
        }

        public void OnUnityAdsAdLoaded(string adUnitId)
        {
            Debug.Log($"[RewardedAds] Ad loaded: {adUnitId}");
            AnalyticsLog("RewardedAdLoaded", adUnitId);
        }

        public void OnUnityAdsFailedToLoad(string adUnitId, UnityAdsLoadError error, string message)
        {
            Debug.LogError($"[RewardedAds] Failed to load ad: {adUnitId}, Error: {error}, Message: {message}");
            AnalyticsLog("RewardedAdLoadFailed", $"{adUnitId}: {error} - {message}");
        }

        public void OnUnityAdsShowFailure(string adUnitId, UnityAdsShowError error, string message)
        {
            Debug.LogError($"[RewardedAds] Failed to show ad: {adUnitId}, Error: {error}, Message: {message}");
            AnalyticsLog("RewardedAdShowFailed", $"{adUnitId}: {error} - {message}");
        }

        public void OnUnityAdsShowStart(string adUnitId)
        {
            Debug.Log($"[RewardedAds] Ad started: {adUnitId}");
            AnalyticsLog("RewardedAdStarted", adUnitId);
        }

        public void OnUnityAdsShowClick(string adUnitId)
        {
            Debug.Log($"[RewardedAds] Ad clicked: {adUnitId}");
            AnalyticsLog("RewardedAdClicked", adUnitId);
        }

        public void OnUnityAdsShowComplete(string adUnitId, UnityAdsShowCompletionState showCompletionState)
        {
            Debug.Log($"[RewardedAds] Ad completed: {adUnitId}, State: {showCompletionState}");
            AnalyticsLog("RewardedAdCompleted", $"{adUnitId}: {showCompletionState}");

            if (adUnitId.Equals(_adUnitId) && showCompletionState.Equals(UnityAdsShowCompletionState.COMPLETED))
            {
                _onAdWatchedCallback?.Invoke();
                _onAdWatchedCallback = null;

                OnDestroy();
            }
        }

        private void AnalyticsLog(string eventName, string parameter)
        {
            FirebaseAnalytics.LogEvent(eventName, new Parameter("info", parameter));
            Debug.Log($"[Analytics] {eventName}: {parameter}");
        }

        void OnDestroy() { }
    }
}
