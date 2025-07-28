using Project.Scripts.Firebase;
using Project.Scripts.GameFlowScripts;
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
