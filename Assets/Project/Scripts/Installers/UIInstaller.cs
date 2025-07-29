using Project.Scripts.Auth;
using Project.Scripts.Players;
using Project.Scripts.Purchaser;
using UnityEngine;
using Zenject;

namespace Project.Scripts.Installers
{
    public class UIInstaller : MonoInstaller
    {
        [SerializeField] private PurchaseView _purchaseView;

        public override void InstallBindings()
        {
            Container.Bind<PurchaseConfig>().AsSingle();
            Container.BindInterfacesAndSelfTo<UnityIAPPurchaser>().AsSingle();
            Container.Bind<PlayerPrefsSave>().AsSingle();
            Container.Bind<PurchaseView>().FromInstance(_purchaseView).AsSingle();
            Container.BindInterfacesAndSelfTo<PurchasePresenter>().AsSingle();
        }
    }
}
