using System;
using Cysharp.Threading.Tasks;
using Project.Scripts.ADS;
using Project.Scripts.GameFlowScripts;

namespace Project.Scripts.PanelSettings
{
    public class PanelADSModel
    {
        private readonly AdsService _adsService;
        private readonly SceneLoader _sceneLoader;
        private readonly Func<UniTask> _revivePlayerCallback;

        public PanelADSModel(AdsService adsService, SceneLoader sceneLoader, Func<UniTask> revivePlayerCallback)
        {
            _adsService = adsService;
            _sceneLoader = sceneLoader;
            _revivePlayerCallback = revivePlayerCallback;
        }

        public void ReloadGame()
        {
            _sceneLoader.ReloadScene();
        }

        public void ShowRewardedAd()
        {
            _adsService.ShowRewardedAd(() => _revivePlayerCallback().Forget());
        }
    }
}
