using Project.Scripts.Purchasers;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Project.Scripts.Installers
{
    public class UIInstaller : MonoInstaller
    {
        [SerializeField] private Button _buyButton;

        public override void InstallBindings()
        {
            Container.Bind<Purchaser>().AsSingle().NonLazy();
            Container.Bind<Button>().FromInstance(_buyButton).AsSingle();
            Container.Bind<PurchaseHandler>().AsSingle().NonLazy();
        }
    }
}
