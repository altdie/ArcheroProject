using Project.Scripts.Players;
using UnityEngine;
using UnityEngine.Purchasing;

namespace Project.Scripts.ADS
{
    public class Purchaser : MonoBehaviour
    {
        public void OnPurchaseCompleted(Product product)
        {
            switch (product.definition.id)
            {
                case "RemoveAdd":
                    RemoveAds();
                    break;
            }
        }

        private void RemoveAds()
        {
            var saveSystem = new PlayerPrefsSave();
            var playerData = saveSystem.Load();
            playerData.IsAdsRemoved = true;
            saveSystem.Save(playerData);
        }
    }
}