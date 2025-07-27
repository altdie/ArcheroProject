using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using Project.Scripts.GameFlowScripts;
using Unity.Services.Authentication;
using Unity.Services.Core;
using UnityEngine;

namespace Project.Scripts.Auth
{
    public class AuthManager
    {
        public async UniTask InitializeAsync()
        {
            try
            {
                await UnityServices.InitializeAsync();

                if (!AuthenticationService.Instance.IsSignedIn)
                    await AuthenticationService.Instance.SignInAnonymouslyAsync();
            }
            catch (AuthenticationException ex)
            {
                Debug.LogError("Авторизация не удалась: " + ex.Message);
            }
        }
    }

}