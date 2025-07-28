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
            _buyButton.onClick.AddListener(() => OnBuyClicked?.Invoke());
        }

        private void OnDestroy()
        {
            _buyButton.onClick.RemoveAllListeners();
        }
    }
}
