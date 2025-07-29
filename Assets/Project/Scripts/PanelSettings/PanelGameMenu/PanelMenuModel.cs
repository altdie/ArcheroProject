using Project.Scripts.GameFlowScripts;
using Unity.Services.Authentication;
using UnityEngine;

namespace Project.Scripts.PanelSettings.PanelGameMenu
{
    public class PanelMenuModel
    {
        private readonly SceneLoader _sceneLoader;

        public PanelMenuModel(SceneLoader sceneLoader)
        {
            _sceneLoader = sceneLoader;
        }

        public void StartGame()
        {
            _sceneLoader.StartGame();
        }

        public void RemoveAds()
        {
            Debug.Log(AuthenticationService.Instance.PlayerId);
        }
    }
}
