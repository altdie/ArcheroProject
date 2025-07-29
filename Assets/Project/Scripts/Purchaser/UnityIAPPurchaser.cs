using System;
using Cysharp.Threading.Tasks;
using Unity.Services.Core;
using Unity.Services.Core.Environments;
using UnityEngine;
using UnityEngine.Purchasing;
using Zenject;

namespace Project.Scripts.Purchaser
{
    public class UnityIAPPurchaser : IStoreListener, IPurchaser, IInitializable
    {
        private const string PRODUCTION = "production";
        private IStoreController _controller;
        private readonly PurchaseConfig _config;
        public event Action<string> OnPurchaseCompleted;

        public UnityIAPPurchaser(PurchaseConfig config)
        {
            _config = config;
        }

        public void Initialize()
        {
            InitializeAsync().Forget();
        }

        private async UniTask InitializeAsync()
        {
            await InitializeUGSAndIAP();
        }

        private async UniTask InitializeUGSAndIAP()
        {
            try
            {
                var options = new InitializationOptions().SetEnvironmentName(PRODUCTION);
                await UnityServices.InitializeAsync(options);

                var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());
                builder.AddProduct(_config.RemoveAdsProductId, ProductType.NonConsumable);
                UnityPurchasing.Initialize(this, builder);
            }
            catch (Exception e)
            {
                Debug.LogError($"[Purchaser] Failed to initialize UGS or IAP: {e.Message}");
            }
        }

        public void Buy(string productId)
        {
            var product = _controller.products.WithID(productId);
            if (product.availableToPurchase)
            {
                _controller.InitiatePurchase(product);
            }
        }

        public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
        {
            _controller = controller;
        }

        public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs args)
        {
            OnPurchaseCompleted?.Invoke(args.purchasedProduct.definition.id);
            return PurchaseProcessingResult.Complete;
        }

        public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason) { }

        public void OnInitializeFailed(InitializationFailureReason error)
        {
            Debug.LogError($"[Purchaser] IAP Initialization Failed: {error}");
        }

        public void OnInitializeFailed(InitializationFailureReason error, string message)
        {
            Debug.LogError($"[Purchaser] IAP Initialization Failed: {error}, Message: {message}");
        }
    }
}