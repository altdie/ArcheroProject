using TMPro;
using Unity.Services.Authentication;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Project.Scripts.GameFlowScripts
{
    public class SceneLoader 
    {
        private readonly Button _startGameButton;
        private readonly TextMeshProUGUI _logTxt;

        public SceneLoader(Button startGameButton, TextMeshProUGUI logTxt)
        {
            _startGameButton = startGameButton;
            _logTxt = logTxt;
            _startGameButton.onClick.AddListener(StartGame);
        }

        public void ReloadScene()
        {
            string currentSceneName = SceneManager.GetActiveScene().name;
            SceneManager.LoadScene(currentSceneName);
            Time.timeScale = 1.0f;
        }

        public void StartGame()
        {
            SceneManager.LoadScene("StartSceneTest");
        }

        public void PlayerAuth()
        {
            _logTxt.text = "Player id:" + AuthenticationService.Instance.PlayerId;
        }
    }
}