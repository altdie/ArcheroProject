using Project.Scripts.GameFlowScripts;
using Project.Scripts.PanelSettings.PanelGameMenu;
using TMPro;
using UnityEngine;
using Zenject;

namespace Project.Scripts.Installers
{
    public class AuthInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            BindManager();
            BindMenuPanel();
        }

        private void BindManager()
        {
            Container.Bind<SceneLoader>().AsSingle();
        }

        private void BindMenuPanel()
        {
            Container.Bind<PanelMenuView>().FromComponentInHierarchy().AsSingle();
            Container.Bind<PanelMenuModel>().AsSingle();
            Container.BindInterfacesAndSelfTo<PanelMenuPresenter>().AsSingle();
        }
    }
}