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
        [SerializeField] private AuthView _authView;
        [SerializeField] private Button _startGameButton;
        [SerializeField] private TextMeshProUGUI _logText;

        public override void InstallBindings()
        {
            BindManager();
        }

        private void BindManager()
        {
            Container.Bind<AuthManager>().AsSingle();
            Container.Bind<AuthView>().FromComponentInHierarchy().AsSingle();
            Container.Bind<AuthPresenter>().AsSingle();
            Container.Bind<SceneLoader>().AsSingle().WithArguments(_startGameButton, _logText);
        }
    }
}