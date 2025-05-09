using Assets.Project.Scripts.ADS;
using UnityEngine;
using UnityEngine.Advertisements;

namespace Project.Scripts.ADS
{
    public class InterstitialAds : IUnityAdsLoadListener, IUnityAdsShowListener
    {
        private readonly string _androidAdUnitId = "Interstitial_Android";
        private readonly string _iOsAdUnitId = "Interstitial_iOS";
        private string _adUnitId;

        public void Initialize()
        {
            _adUnitId = (Application.platform == RuntimePlatform.IPhonePlayer)
                ? _iOsAdUnitId
                : _androidAdUnitId;
        }

        public void LoadAd()
        {
            Advertisement.Load(_adUnitId, this);
        }

        public void ShowAd()
        {
            Advertisement.Show(_adUnitId, this);
        }

        public void OnUnityAdsAdLoaded(string placementId) { }

        public void OnUnityAdsFailedToLoad(string placementId, UnityAdsLoadError error, string message) { }

        public void OnUnityAdsShowClick(string placementId) { }
        public void OnUnityAdsShowComplete(string placementId, UnityAdsShowCompletionState showCompletionState) { }

        public void OnUnityAdsShowFailure(string placementId, UnityAdsShowError error, string message) { }

        public void OnUnityAdsShowStart(string placementId) { }

    }
}
