using System;
using Cysharp.Threading.Tasks;
using Unity.Services.Core;
using Unity.Services.Core.Environments;
using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.Purchasing.Extension;
using Zenject;

namespace Project.Scripts.Purchaser
{
    public class UnityIAPPurchaser : IPurchaser, IInitializable, IDetailedStoreListener
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
            InitializeUGSAndIAP().Forget();
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
            Debug.Log("[Purchaser] IAP successfully initialized.");
            _controller = controller;
        }

        public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs args)
        {
            Debug.Log($"[Purchaser] Purchase successful: {args.purchasedProduct.definition.id}");
            OnPurchaseCompleted?.Invoke(args.purchasedProduct.definition.id);
            return PurchaseProcessingResult.Complete;
        }

        public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
        {
            Debug.LogWarning($"[Purchaser] Purchase failed: {product.definition.id}, Reason: {failureReason}");
        }

        public void OnInitializeFailed(InitializationFailureReason error)
        {
            Debug.LogError($"[Purchaser] IAP Initialization Failed: {error}");
        }

        public void OnInitializeFailed(InitializationFailureReason error, string message)
        {
            Debug.LogError($"[Purchaser] IAP Initialization Failed: {error}, Message: {message}");
        }

        public void OnPurchaseFailed(Product product, PurchaseFailureDescription failureDescription)
        {
            Debug.LogWarning($"[Purchaser] Purchase failed: {product.definition.id}, Reason: {failureDescription.reason}, Message: {failureDescription.message}");
        }
    }
}