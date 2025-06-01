using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Project.Scripts.Auth
{
    public class AuthView : MonoBehaviour
    {
        [SerializeField] private Button _signInButton;

        private AuthPresenter _presenter;

        [Inject]
        public void Construct(AuthPresenter presenter)
        {
            _presenter = presenter;
        }

        private void Start()
        {
            _signInButton.onClick.AddListener(OnSignInClicked);
        }

        private void OnSignInClicked()
        {
            _presenter.OnSignInClicked();
        }
    }
}