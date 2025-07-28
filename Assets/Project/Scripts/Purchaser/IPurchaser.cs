using System;

namespace Project.Scripts.Purchaser
{
    public interface IPurchaser
    {
        event Action<string> OnPurchaseCompleted;
        void Init();
        void Buy(string productId);
    }
}
