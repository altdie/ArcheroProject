using Project.Scripts.Players;
using UnityEngine;
using UnityEngine.UI;

namespace Project.Scripts.Purchasers
{
    public class PurchaseHandler
    {
        private readonly PlayerPrefsSave _save;

        public PurchaseHandler(Purchaser purchaser, Button buyButton, PlayerPrefsSave save)
        {
            purchaser.OnPurchaseCompleted += OnPurchaseSuccess;
            purchaser.Init();

            buyButton.onClick.AddListener(() => purchaser.Buy("RemoveAdd"));
            _save = save;
        }

        private void OnPurchaseSuccess(string productId)
        {
            if (productId == "RemoveAdd")
            {
                var data = _save.Load();
                data.IsAdsRemoved = true;
                _save.Save(data);
                Debug.Log("[PurchaseHandler] Saved IsAdsRemoved: true");
            }
        }
    }
}
