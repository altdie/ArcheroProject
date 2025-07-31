using System;
using Project.Scripts.Players;
using Zenject;

namespace Project.Scripts.Purchaser
{
    public class PurchasePresenter : IInitializable, IDisposable
    {
        private readonly IPurchaser _purchaser;
        private readonly PurchaseView _view;
        private readonly PlayerPrefsSave _save;
        private readonly PurchaseConfig _purchaseConfig;

        public PurchasePresenter(IPurchaser purchaser, PurchaseView view, PlayerPrefsSave save, PurchaseConfig purchaseConfig)
        {
            _purchaser = purchaser;
            _view = view;
            _save = save;
            _purchaseConfig = purchaseConfig;
        }

        public void Initialize()
        {
            _purchaser.OnPurchaseCompleted += OnPurchaseSuccess;
            _view.OnBuyClicked += OnBuyClicked;
        }

        public void Dispose()
        {
            _purchaser.OnPurchaseCompleted -= OnPurchaseSuccess;
            _view.OnBuyClicked -= OnBuyClicked;
        }

        private void OnBuyClicked()
        {
            _purchaser.Buy(_purchaseConfig.RemoveAdsProductId);
        }

        private void OnPurchaseSuccess(string productId)
        {
            if (productId == _purchaseConfig.RemoveAdsProductId)
            {
                var data = _save.Load();
                data.IsAdsRemoved = true;
                _save.Save(data);
            }
        }
    }

}
