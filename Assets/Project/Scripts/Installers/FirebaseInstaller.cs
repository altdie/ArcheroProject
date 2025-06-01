using Project.Scripts.Firebase;
using Zenject;

namespace Project.Scripts.Installers
{
    public class FirebaseInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<IAnalyticsService>()
                .To<FirebaseAnalyticsService>()
                .AsSingle()
                .NonLazy();
        }
    }
}
