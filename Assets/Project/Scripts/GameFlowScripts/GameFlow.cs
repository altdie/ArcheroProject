using Cysharp.Threading.Tasks;
using Project.Scripts.ADS;
using Project.Scripts.Audio;
using Project.Scripts.Auth;
using Project.Scripts.Enemies;
using Project.Scripts.Firebase;
using Project.Scripts.Installers;
using Project.Scripts.NextLevel;
using Project.Scripts.Players;
using Project.Scripts.SaveSystem;
using Zenject;

namespace Project.Scripts.GameFlowScripts
{
    public class GameFlow : IInitializable
    {
        private readonly IAnalyticsService _analyticsService;
        private readonly AdsInitializer _adsInitializer;
        private readonly SaveSelection _saveSelection;
        private readonly IDoorView _doorView;
        private readonly AdsService _adsService;
        private readonly AuthManager _authManager;
        private readonly AudioManager _audioManager;
        private readonly PlayerLifecycle _playerLifecycle;
        private readonly EnemyLifecycleManager _enemyLifecycleManager;
        private readonly UIFlow _uiFlow;

        public GameFlow(
            IAnalyticsService analyticsService,
            AdsInitializer adsInitializer,
            SaveSelection saveSelection,
            IDoorView doorView,
            AdsService adsService,
            AuthManager authManager,
            AudioManager audioManager,
            PlayerLifecycle playerLifecycle,
            EnemyLifecycleManager enemyLifecycleManager)
        {
            _analyticsService = analyticsService;
            _adsInitializer = adsInitializer;
            _saveSelection = saveSelection;
            _doorView = doorView;
            _adsService = adsService;
            _authManager = authManager;
            _audioManager = audioManager;
            _playerLifecycle = playerLifecycle;
            _enemyLifecycleManager = enemyLifecycleManager;
        }

        public void Initialize()
        {
            InitializeGameFlow().Forget();
        }

        private async UniTask InitializeGameFlow()
        {
            await InitializeCoreSystems();

            _enemyLifecycleManager.SetPlayer(_playerLifecycle.Player);
            _enemyLifecycleManager.SetOnAllEnemiesDefeatedCallback(OnAllEnemiesDefeated);
            await _enemyLifecycleManager.Initialize();

            _adsInitializer.InitializeAds();
            _adsService.LoadInterstitialAd();
            _adsService.LoadRewardedAd();
            _doorView.Disable();
            _audioManager.PlayBackgroundMusic();
        }

        private async UniTask InitializeCoreSystems()
        {
            await UniTask.WhenAll(
                _authManager.Initialize(),
                _saveSelection.Initialize(),
                _playerLifecycle.InitializePlayer());
        }

        private void OnAllEnemiesDefeated()
        {
            _doorView.Enable();
            _playerLifecycle.Player.Level++;
            _analyticsService.LogLevelPassed(_playerLifecycle.Player.Level);
            _uiFlow.UpdatePlayerStatsUI();
            _ = _saveSelection.Save(_playerLifecycle.Player);
        }
    }
}
