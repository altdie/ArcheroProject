using System;
using Project.Scripts.Players;
using Zenject;

namespace Project.Scripts.Purchaser
{
    public class PurchasePresenter : IInitializable, IDisposable
    {
        private const string REMOVE_ADD_PRODUCT = "REMOVEADS";
        private readonly IPurchaser _purchaser;
        private readonly PurchaseView _view;
        private readonly PlayerPrefsSave _save;

        public PurchasePresenter(IPurchaser purchaser, PurchaseView view, PlayerPrefsSave save)
        {
            _purchaser = purchaser;
            _view = view;
            _save = save;
        }

        public void Initialize()
        {
            _purchaser.OnPurchaseCompleted += OnPurchaseSuccess;
            _view.OnBuyClicked += OnBuyClicked;

            _purchaser.Init();
        }

        public void Dispose()
        {
            _purchaser.OnPurchaseCompleted -= OnPurchaseSuccess;
            _view.OnBuyClicked -= OnBuyClicked;
        }

        private void OnBuyClicked()
        {
            _purchaser.Buy(REMOVE_ADD_PRODUCT);
        }

        private void OnPurchaseSuccess(string productId)
        {
            if (productId == REMOVE_ADD_PRODUCT)
            {
                var data = _save.Load();
                data.IsAdsRemoved = true;
                _save.Save(data);
            }
        }
    }

}
