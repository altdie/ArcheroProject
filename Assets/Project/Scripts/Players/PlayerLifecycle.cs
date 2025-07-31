using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Project.Scripts.ADS;
using Project.Scripts.Firebase;
using Project.Scripts.GameFlowScripts;
using Project.Scripts.PanelSettings;
using Project.Scripts.SaveSystem;
using Project.Scripts.UI;

namespace Project.Scripts.Players
{
    public class PlayerLifecycle : IDisposable
    {
        public PlayerModel Player { get; private set; }

        private readonly PlayerPrefsSave _playerPrefsSave;
        private readonly PlayerFactory _playerFactory;
        private readonly PlayerSpawnPoint _spawnPointPlayer;
        private readonly Joystick _joystick;
        private readonly IAnalyticsService _analyticsService;
        private readonly TimeService _timeService;
        private readonly SaveSelection _saveSelection;
        private readonly SceneLoader _sceneLoader;
        private readonly AdsService _adsService;
        private readonly PlayerConfig _playerConfig;
        private readonly UIFlow _uiFlow;

        private PlayerStatsUIPresenter _playerStatsUIPresenter;
        private PlayerStatsUIModel _playerStatsUIModel;
        private CancellationTokenSource _cts;
        private CancellationToken _token;
        private PanelADSPresenter _panelPresenter;
        private bool _rewardAdsComplete;

        public PlayerLifecycle(
            PlayerFactory playerFactory,
            PlayerSpawnPoint spawnPointPlayer,
            Joystick joystick,
            IAnalyticsService analyticsService,
            TimeService timeService,
            SaveSelection saveSelection,
            SceneLoader sceneLoader,
            AdsService adsService,
            PlayerPrefsSave playerPrefsSave,
            PlayerConfig playerConfig, 
            UIFlow uiFlow)
        {
            _playerFactory = playerFactory;
            _spawnPointPlayer = spawnPointPlayer;
            _joystick = joystick;
            _analyticsService = analyticsService;
            _timeService = timeService;
            _saveSelection = saveSelection;
            _sceneLoader = sceneLoader;
            _adsService = adsService;
            _playerPrefsSave = playerPrefsSave;
            _playerConfig = playerConfig;
            _uiFlow = uiFlow;
        }

        public async UniTask InitializePlayer()
        {
            CreateCancellationToken();
            Player = await _playerFactory.CreatePlayer(_spawnPointPlayer, _playerConfig.PlayerInitialHealth, _joystick);
            _uiFlow.Initialize(Player, Player.Experience);
            Player.OnDeath += OnPlayerDeath;
            await LoadPlayerData(_token);
        }

        private void CreateCancellationToken()
        {
            _cts = new CancellationTokenSource();
            _token = _cts.Token;
        }

        private async UniTaskVoid RemovePlayer()
        {
            _timeService.PauseAttack();
            _adsService.ShowInterstitialAd();

            if (!_rewardAdsComplete)
            {
                await _uiFlow.ShowFreeLifePanel(_token, _adsService, _sceneLoader, RevivePlayer);
            }
            else
            {
                await ClearData(_token);
                await LoadPlayerData(_token);
                LogDeathAnalytics();
                await _uiFlow.ShowEndGamePanel(_token, _adsService, _sceneLoader, RevivePlayer);
            }
        }

        private void OnPlayerDeath() => _ = RemovePlayer();

        private async UniTask RevivePlayer()
        {
            _rewardAdsComplete = true;
            _playerPrefsSave.Load();
            Player = await _playerFactory.CreatePlayer(_spawnPointPlayer, _playerConfig.PlayerInitialHealth, _joystick);
            Player.OnDeath += OnPlayerDeath;
            _uiFlow.DestroyPanels();
            _timeService.ResumeAttack();
        }

        private async UniTask LoadPlayerData(CancellationToken token)
        {
            token.ThrowIfCancellationRequested();
            var savedData = await _saveSelection.Load(token);
            Player.Experience = savedData.Experience;
            Player.IsAdsRemoved = savedData.IsAdsRemoved;
        }

        private async UniTask ClearData(CancellationToken token) =>
            await _saveSelection.Clear(token);

        private void LogDeathAnalytics() =>
            _analyticsService.LogEntityDeath(Player.CurrentWeapon.BulletsFired);

        public void Dispose()
        {
            Player.OnDeath -= OnPlayerDeath;
            Player.UnsubscribeFromHealthChanged();
            _cts?.Cancel();
            _cts?.Dispose();
        }
    }
}
