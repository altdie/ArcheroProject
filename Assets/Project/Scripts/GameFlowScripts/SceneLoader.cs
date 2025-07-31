using UnityEngine.SceneManagement;

namespace Project.Scripts.GameFlowScripts
{
    public class SceneLoader
    {
        private const string GAMEPLAY_SCENE_NAME = "StartSceneTest";
        
        public void ReloadScene()
        {
            string currentSceneName = SceneManager.GetActiveScene().name;
            SceneManager.LoadScene(currentSceneName);
        }

        public void StartGame()
        {
            SceneManager.LoadScene(GAMEPLAY_SCENE_NAME);
        }
    }
}