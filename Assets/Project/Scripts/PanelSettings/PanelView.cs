using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Project.Scripts.PanelSettings
{
    public class PanelView : MonoBehaviour
    {
        [SerializeField] private Button _reloadGameButton;
        [SerializeField] private Button _rewardedAdsButton;
        [SerializeField] private CanvasGroup _canvasGroup;

        private RectTransform _rectTransform;

        public event Action ReloadGameClicked;
        public event Action RewardedAdsClicked;

        private void Awake()
        {
            _rectTransform = GetComponent<RectTransform>();
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
            ReloadGameClicked?.Invoke();
        }

        private void OnRewardedAdsClicked()
        {
            RewardedAdsClicked?.Invoke();
        }

        public void AnimateIn()
        {
            _rectTransform.localScale = Vector3.one * 0.5f;
            _canvasGroup.alpha = 0f;

            Sequence sequence = DOTween.Sequence();
            sequence.Append(_rectTransform.DOScale(1f, 3f).SetEase(Ease.OutQuad));
            sequence.Join(_canvasGroup.DOFade(1f, 3f).SetEase(Ease.InOutQuad));
        }
    }
}
