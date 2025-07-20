using Project.Scripts.Firebase;
using Project.Scripts.GameFlowScripts;
using Project.Scripts.Players;
using UnityEngine;
using Zenject;

namespace Project.Scripts.Installers
{
    public class FirebaseInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesTo<FirebaseAnalyticsService>().AsSingle();
            Container.BindInterfacesTo<EntryPoint>().AsSingle();
        }
    }
}
