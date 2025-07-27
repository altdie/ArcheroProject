using System;

public interface IPurchaser
{
    event Action<string> OnPurchaseCompleted;
    void Init();
    void Buy(string productId);
}
