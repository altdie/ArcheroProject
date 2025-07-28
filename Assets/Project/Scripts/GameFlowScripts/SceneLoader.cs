using UnityEngine;
using UnityEngine.SceneManagement;

namespace Project.Scripts.GameFlowScripts
{
    public class SceneLoader
    {
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
    }
}