using Project.Scripts.Auth;
using Project.Scripts.Players;
using Project.Scripts.SaveSystem;
using UnityEngine;
using Zenject;

namespace Project.Scripts.Installers
{
    public class GlobalInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<PlayerDataSave>().AsSingle().NonLazy();
            Container.Bind<PlayerPrefsSave>().AsSingle().NonLazy();
            Container.Bind<AuthManager>().AsSingle();
            Container.Bind<CloudSave>().AsSingle();
            Container.Bind<SaveSelection>().AsSingle();
        }

        public override void Start()
        {
            var authManager = Container.Resolve<AuthManager>();
            _ = authManager.InitializeAsync();
        }
    }
}
