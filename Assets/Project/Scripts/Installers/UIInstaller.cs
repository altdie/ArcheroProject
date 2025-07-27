using Project.Scripts.Players;
using Project.Scripts.Purchasers;
using System.ComponentModel;
using UnityEngine;
using Zenject;

public class UIInstaller : MonoInstaller
{
    [SerializeField] private PurchaseView _purchaseView;

    public override void InstallBindings()
    {
        Container.Bind<IPurchaser>().To<UnityIAPPurchaser>().AsSingle();
        Container.Bind<PlayerPrefsSave>().AsSingle();
        Container.Bind<PurchaseView>().FromInstance(_purchaseView).AsSingle();
        Container.BindInterfacesAndSelfTo<PurchasePresenter>().AsSingle();
    }
}
