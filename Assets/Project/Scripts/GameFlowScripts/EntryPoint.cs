using UnityEngine.SceneManagement;
using Zenject;

namespace Project.Scripts.GameFlowScripts
{
    public class EntryPoint : IInitializable
    {
        private readonly string _menuSceneName = "MenuScene";

        public void Initialize()
        {
            SceneManager.LoadScene(_menuSceneName);
        }
    }
}