using System.Threading.Tasks;
using Assets.Project.Scripts.ADS;
using Project.Scripts.Addressables;
using Project.Scripts.GameFlowScripts;
using UnityEngine;

namespace Project.Scripts.PanelSettings
{
    public class PanelModel
    {
        private readonly Collider _door;
        private readonly IAssetProvider _assetProvider;
        private readonly Canvas _canvas;
        private readonly SceneLoader _sceneLoader;
        private readonly IAds _ads;
        private PanelView _currentPanelView;

        public PanelModel(Collider door, IAssetProvider assetProvider, Canvas canvas, SceneLoader sceneLoader, PanelView currentPanelView, IAds ads)
        {
            _door = door;
            _assetProvider = assetProvider;
            _canvas = canvas;
            _sceneLoader = sceneLoader;
            _currentPanelView = currentPanelView;
            _ads = ads;
        }

        public void ReloadScene()
        {
            ShowInterstitialAd();
            _sceneLoader.ReloadScene();
        }

        public void ShowRewardedAd()
        {
            _ads.LoadRewardedAd();
            _ads.ShowRewardedAd();
        }

        private void ShowInterstitialAd()
        {
            _ads.ShowInterstitialAd();
        }

        public async Task CreatePanelAsync()
        {
            _currentPanelView = await _assetProvider.LoadPanelPrefabAsync();
            _currentPanelView.transform.SetParent(_canvas.transform, false);

            _currentPanelView.ReloadGameClicked += ReloadScene;
            _currentPanelView.RewardedAdsClicked += ShowRewardedAd;

            _currentPanelView.gameObject.SetActive(true);
        }

        public void EnableCollider()
        {
            _door.enabled = true;
        }

        public void DisablePanels()
        {
            _door.enabled = false;
            _currentPanelView.gameObject.SetActive(false);
        }

    }
}