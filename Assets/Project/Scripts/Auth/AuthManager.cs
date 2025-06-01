using System.Threading.Tasks;
using Project.Scripts.GameFlowScripts;
using Unity.Services.Authentication;
using Unity.Services.Core;
using UnityEngine;

namespace Project.Scripts.Auth
{
    public class AuthManager
    {
        private readonly SceneLoader _sceneLoader;

        public AuthManager(SceneLoader sceneLoader)
        {
            _sceneLoader = sceneLoader;
        }

        public void SignIn()
        {
            _ = SignInAnonymous();
        }

        private async Task SignInAnonymous()
        {
            try
            {
                await UnityServices.InitializeAsync();
                await AuthenticationService.Instance.SignInAnonymouslyAsync();

                _sceneLoader.PlayerAuth();
            }
            catch (AuthenticationException ex)
            {
                Debug.LogException(ex);
            }
        }
    }
}