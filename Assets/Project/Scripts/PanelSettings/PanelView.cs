using System;
using System.Threading.Tasks;
using Project.Scripts.Addressables;
using UnityEngine;
using UnityEngine.UI;

namespace Project.Scripts.PanelSettings
{
    public class PanelView : MonoBehaviour
    {
        [SerializeField] private Button _reloadGameButton;
        [SerializeField] private Button _rewardedAdsButton;

        public event Action ReloadGameClicked;
        public event Action RewardedAdsClicked;

        public void Awake()
        {
            _reloadGameButton.onClick.AddListener(OnReloadGameClicked);
            _rewardedAdsButton.onClick.AddListener(OnRewardedAdsClicked);
        }

        private void OnDestroy()
        {
            _reloadGameButton.onClick.RemoveListener(OnReloadGameClicked);
            _rewardedAdsButton.onClick.RemoveListener(OnRewardedAdsClicked);
        }

        private void OnReloadGameClicked()
        {
            Debug.Log("🔴 Нажата кнопка Reload");
            ReloadGameClicked?.Invoke();
        }

        private void OnRewardedAdsClicked()
        {
            Debug.Log("🔵 Нажата кнопка Free Life");
            RewardedAdsClicked?.Invoke();
        }
    }
}