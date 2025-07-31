using Cysharp.Threading.Tasks;
using Firebase;
using Firebase.Analytics;
using Zenject;

namespace Project.Scripts.Firebase
{
    public class FirebaseAnalyticsService : IAnalyticsService, IInitializable
    {
        public void Initialize()
        {
            _ = InitAsync();
        }

        public async UniTask InitAsync()
        {
           await FirebaseApp.CheckAndFixDependenciesAsync();
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