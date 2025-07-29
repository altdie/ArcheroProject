using System;
using UnityEngine;
using UnityEngine.UI;

namespace Project.Scripts.Purchaser
{
    public class PurchaseView : MonoBehaviour
    {
        [SerializeField] private Button _buyButton;

        public event Action OnBuyClicked;

        private void Awake()
        {
            _buyButton.onClick.AddListener(OnBuyButtonClicked);
        }

        private void OnDestroy()
        {
            _buyButton.onClick.RemoveListener(OnBuyButtonClicked);
        }

        private void OnBuyButtonClicked()
        {
            OnBuyClicked?.Invoke();
        }
    }
}
