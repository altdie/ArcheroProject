using System;
using Project.Scripts.GameFlowScripts;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Project.Scripts.PanelSettings
{
    public class PanelPresenter
    {
        private readonly PanelView _view;
        private readonly GameFlow _gameFlow;
        private readonly SceneLoader _sceneLoader;

        public PanelPresenter(PanelView view, GameFlow gameFlow, SceneLoader sceneLoader)
        {
            _view = view;
            _gameFlow = gameFlow;
            _sceneLoader = sceneLoader;

            _view.ReloadGameClicked += OnReloadClicked;
            _view.RewardedAdsClicked += OnFreeLifeClicked;
        }

        private void OnReloadClicked()
        {
            _sceneLoader.ReloadScene();
        }

        private void OnFreeLifeClicked()
        {
            Debug.Log("❤️ Обработка кнопки Free Life в Presenter");
            _gameFlow.ShowRewardedAdCallback();
        }

        public void Dispose()
        {
            _view.ReloadGameClicked -= OnReloadClicked;
            _view.RewardedAdsClicked -= OnFreeLifeClicked;
        }
    }
}