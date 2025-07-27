using System.ComponentModel;
using Project.Scripts.GameFlowScripts;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Project.Scripts.Auth
{
    public class AuthInstaller : MonoInstaller
    {
        [SerializeField] private TextMeshProUGUI _text;

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
            Container.Bind<PanelMenuModel>().AsSingle().WithArguments(_text);
            Container.BindInterfacesAndSelfTo<PanelMenuPresenter>().AsSingle();
        }
    }
}