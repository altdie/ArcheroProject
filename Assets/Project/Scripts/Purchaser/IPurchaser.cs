using System;
using Cysharp.Threading.Tasks;

namespace Project.Scripts.Purchaser
{
    public interface IPurchaser
    {
        event Action<string> OnPurchaseCompleted;
        void Buy(string productId);
    }
}
