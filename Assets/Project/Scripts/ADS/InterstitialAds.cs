using Firebase.Analytics;
using Project.Scripts.Players;
using UnityEngine;
using UnityEngine.Advertisements;

namespace Project.Scripts.ADS
{
    public class InterstitialAds : IUnityAdsLoadListener, IUnityAdsShowListener
    {
        private readonly string _androidAdUnitId = "Interstitial_Android";
        private readonly string _iOsAdUnitId = "Interstitial_iOS";
        private string _adUnitId;
        private PlayerPrefsSave _save;

        public void Initialize(PlayerPrefsSave save)
        {
            _adUnitId = (Application.platform == RuntimePlatform.IPhonePlayer)
                ? _iOsAdUnitId
                : _androidAdUnitId;

            _save = save;
        }

        public void LoadAd()
        {
            Advertisement.Load(_adUnitId, this);
        }

        public void ShowAd()
        {
            var data = _save.Load();

            if (data.IsAdsRemoved)
            {
                return;
            }
            Advertisement.Show(_adUnitId, this);
        }

        public void OnUnityAdsAdLoaded(string placementId)
        {
            Debug.Log($"[InterstitialAds] Ad loaded: {placementId}");
            AnalyticsLog("InterstitialAdLoaded", placementId);
        }

        public void OnUnityAdsFailedToLoad(string placementId, UnityAdsLoadError error, string message)
        {
            Debug.LogError($"[InterstitialAds] Failed to load ad: {placementId}, Error: {error}, Message: {message}");
            AnalyticsLog("InterstitialAdLoadFailed", $"{placementId}: {error} - {message}");
        }

        public void OnUnityAdsShowClick(string placementId)
        {
            Debug.Log($"[InterstitialAds] Ad clicked: {placementId}");
            AnalyticsLog("InterstitialAdClicked", placementId);
        }

        public void OnUnityAdsShowComplete(string placementId, UnityAdsShowCompletionState showCompletionState)
        {
            Debug.Log($"[InterstitialAds] Ad completed: {placementId}, State: {showCompletionState}");
            AnalyticsLog("InterstitialAdCompleted", $"{placementId}: {showCompletionState}");
        }

        public void OnUnityAdsShowFailure(string placementId, UnityAdsShowError error, string message)
        {
            Debug.LogError($"[InterstitialAds] Failed to show ad: {placementId}, Error: {error}, Message: {message}");
            AnalyticsLog("InterstitialAdShowFailed", $"{placementId}: {error} - {message}");
        }

        public void OnUnityAdsShowStart(string placementId)
        {
            Debug.Log($"[InterstitialAds] Ad started: {placementId}");
            AnalyticsLog("InterstitialAdStarted", placementId);
        }

        private void AnalyticsLog(string eventName, string parameter)
        {
            FirebaseAnalytics.LogEvent(eventName, new Parameter("info", parameter));
            Debug.Log($"[Analytics] {eventName}: {parameter}");
        }
    }
}
