using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Project.Scripts.PanelSettings.PanelGameMenu
{
    public class PanelMenuView : MonoBehaviour
    {
        [SerializeField] private Button _startGameButton;
        [SerializeField] private Button _removeAdsButton;

        public event Action StartGameClicked;
        public event Action RemoveAdsClicked;

        public void Awake()
        {
            _startGameButton.onClick.AddListener(OnStartGameClicked);
            _removeAdsButton.onClick.AddListener(OnRemoveADSClicked);
        }

        private void OnDestroy()
        {
            _startGameButton.onClick.RemoveListener(OnStartGameClicked);
            _removeAdsButton.onClick.RemoveListener(OnRemoveADSClicked);
        }

        private void OnStartGameClicked()
        {
            StartGameClicked?.Invoke();
        }

        private void OnRemoveADSClicked()
        {
            RemoveAdsClicked?.Invoke();
        }
    }
}
