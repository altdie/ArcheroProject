using Firebase;
using Firebase.Analytics;
using Firebase.Extensions;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Project.Scripts.Firebase
{
    public class FirebaseAnalyticsService : IAnalyticsService
    {
        private readonly string _menuSceneName = "MenuScene";
        private bool _initialized;

        public FirebaseAnalyticsService()
        {
            Initialize();
        }

        public async void Initialize()
        {
            if (_initialized) return;

            var dependencyResult = await FirebaseApp.CheckAndFixDependenciesAsync();
            if (dependencyResult == DependencyStatus.Available)
            {
                Debug.Log("Firebase initialized. Loading menu...");
                _initialized = true;
      //          SceneManager.LoadScene(_menuSceneName); // если это оставить то всегда при любом переходе сцены будет закидывать на сцену меню
            }
            else
            {
                Debug.LogError($"Firebase dependencies not available: {dependencyResult}");
            }
        }

        public void LogEnemyDeath(int killsCount)
        {
            FirebaseAnalytics.LogEvent("enemy_death", new Parameter("kills_count", killsCount));
        }

        public void LogEntityDeath(int bulletsFired)
        {
            FirebaseAnalytics.LogEvent("entity_death", new Parameter("bullets_fired", bulletsFired));
        }

        public void LogLevelPassed(int levelCount)
        {
            FirebaseAnalytics.LogEvent("level_passed", new Parameter("levels_number", levelCount));
        }
    }
}
