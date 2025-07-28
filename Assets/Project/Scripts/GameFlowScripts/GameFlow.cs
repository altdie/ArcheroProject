using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using Project.Scripts.ADS;
using Project.Scripts.Audio;
using Project.Scripts.Auth;
using Project.Scripts.Enemies;
using Project.Scripts.Firebase;
using Project.Scripts.Installers;
using Project.Scripts.NextLevel;
using Project.Scripts.PanelSettings;
using Project.Scripts.PlayerModels;
using Project.Scripts.Players;
using Project.Scripts.SaveSystem;
using Project.Scripts.UI;
using Zenject;

namespace Project.Scripts.GameFlowScripts
{
    public class GameFlow : IInitializable, IDisposable
    {
        private int _killsCount;
        private bool _rewardAdsComplete = false;

        private List<EnemyModel> _enemies;
        private PlayerModel _player;

        private readonly PlayerPrefsSave _playerPrefsSave;
        private readonly EnemyFactory _enemyFactory;
        private readonly PlayerFactory _playerFactory;
        private readonly PlayerSpawnPoint _spawnPointPlayer;
        private readonly Joystick _joystick;
        private readonly EnemySpawnData[] _enemySpawnData;
        private readonly SceneData _sceneData;
        private readonly IAnalyticsService _analyticsService;
        private readonly AdsInitializer _adsInitializer;
        private readonly TimeService _timeService;
        private readonly SaveSelection _saveSelection;
        private readonly PanelFactory _panelFactory;
        private readonly IDoorView _doorView;
        private readonly SceneLoader _sceneLoader;
        private readonly PlayerStatsUIView _playerStatsUIView;
        private readonly AdsService _adsService;
        private readonly AuthManager _authManager;
        private readonly AudioManager _audioManager;
        private PlayerStatsUIPresenter _playerStatsUIPresenter;
        private PlayerStatsUIModel _playerStatsUIModel;
        private CancellationTokenSource _cts;
        private CancellationToken _token;
        private PanelADSPresenter _panelPresenter;

        public GameFlow(
            EnemyFactory enemyFactory,
            PlayerFactory playerFactory,
            PlayerSpawnPoint spawnPointPlayer,
            Joystick joystick,
            EnemySpawnData[] enemySpawnData,
            SceneData sceneData,
            IAnalyticsService analyticsService,
            AdsInitializer adsInitializer,
            InterstitialAds interstitialAdExample,
            TimeService timeService,
            SaveSelection saveSelection,
            PanelFactory panelFactory,
            IDoorView doorView, PlayerStatsUIView playerStatsUIView,
            SceneLoader sceneLoader, AdsService adsService, PlayerPrefsSave playerPrefsSave, AuthManager authManager, AudioManager audioManager)
        {
            _enemyFactory = enemyFactory;
            _playerFactory = playerFactory;
            _spawnPointPlayer = spawnPointPlayer;
            _joystick = joystick;
            _enemySpawnData = enemySpawnData;
            _sceneData = sceneData;
            _analyticsService = analyticsService;
            _adsInitializer = adsInitializer;
            _timeService = timeService;
            _saveSelection = saveSelection;
            _panelFactory = panelFactory;
            _doorView = doorView;
            _playerStatsUIView = playerStatsUIView;
            _sceneLoader = sceneLoader;
            _adsService = adsService;
            _playerPrefsSave = playerPrefsSave;
            _authManager = authManager;
            _audioManager = audioManager;
        }

        public async void Initialize()
        {
            await InitializeAsync();
            _adsInitializer.InitializeAds();
            _adsService.LoadInterstitialAd();
            _adsService.LoadRewardedAd();
            _rewardAdsComplete = false;
            _doorView.Disable();
            _audioManager.PlayBackgroundMusic();
        }

        private async UniTask InitializeAsync()
        {
            CreateCancellationToken();
            _player = await _playerFactory.CreatePlayerAsync(_spawnPointPlayer, 100, _joystick);
            _enemyFactory.CreateEnemies(_enemySpawnData);

            foreach (var enemy in _enemyFactory.Enemies)
            {
                enemy.OnDeath += () => RemoveEnemy(enemy).Forget();
            }

            _enemies = _enemyFactory.Enemies;
            _playerStatsUIModel = new PlayerStatsUIModel(_player, _sceneData.MaxExperience);
            _playerStatsUIPresenter = new PlayerStatsUIPresenter(_playerStatsUIModel, _playerStatsUIView);
            _playerStatsUIPresenter.Initialize();
            _player.OnDeath += OnPlayerDeath;

            await _authManager.InitializeAsync();
            await LoadPlayerDataAsync(_token);
            await _saveSelection.InitializeAsync();
        }

        private async UniTask LoadPlayerDataAsync(CancellationToken token)
        {
            PlayerDataSave savedData = await _saveSelection.LoadAsync();
            _player.Experience = savedData.Experience;
            _player.IsAdsRemoved = savedData.IsAdsRemoved;
            token.ThrowIfCancellationRequested();
        }

        private async UniTaskVoid RemoveEnemy(EnemyModel enemy)
        {
            _enemies.Remove(enemy);
            _player.PlayerMovement.AddExperience(enemy.EXP);

            await _saveSelection.SaveAsync(_player);
            _killsCount++;

            if (_enemies.Count == 0)
            {
                OnAllEnemiesDefeated();
                _analyticsService.LogEnemyDeath(_killsCount);
            }
        }

        private void OnAllEnemiesDefeated()
        {
            _doorView.Enable();
            LevelUp();
        }

        private async UniTaskVoid RemovePlayer()
        {
            _timeService.PauseAttack();
            _adsService.ShowInterstitialAd();

            switch (_rewardAdsComplete)
            {
                case false:
                    PanelView panelView = await _panelFactory.CreatePanelFreeLife(_token);
                    panelView.AnimateIn();
                    var panelModel = new PanelADSModel(_adsService, _sceneLoader, RevivePlayer);
                    _panelPresenter = new PanelADSPresenter(panelView, panelModel);
                    _panelPresenter.SubscribeOnClick();
                    break;

                case true:                  
                    await ClearData(_token);
                    await LoadPlayerDataAsync(_token);
                    LogDeathAnalytics();

                    PanelView panelEndGame = await _panelFactory.CreatePanelEndGame(_token);
                    panelEndGame.AnimateIn();
                    var panelModelEndGame = new PanelADSModel(_adsService, _sceneLoader, RevivePlayer);
                    _panelPresenter = new PanelADSPresenter(panelEndGame, panelModelEndGame);
                    _panelPresenter.SubscribeOnClick();
                    break;
            }
        }

        private void OnPlayerDeath()
        {
            _ = RemovePlayer();
        }

        public async UniTask RevivePlayer()
        {
            _rewardAdsComplete = true;
            _playerPrefsSave.Load();
            _player = await _playerFactory.CreatePlayerAsync(_spawnPointPlayer, 100, _joystick);
            _player.OnDeath += OnPlayerDeath;
            _panelFactory.DestroyPanel();
            _timeService.ResumeAttack();
        }

        private void LevelUp()
        {
            _player.Level++;
            _playerStatsUIPresenter.UpdateView();

            _analyticsService.LogLevelPassed(_player.Level);
            _ = _saveSelection.SaveAsync(_player);
        }

        public async UniTask ClearData(CancellationToken token)
        {
            await _saveSelection.ClearAsync(token);
        }

        private void LogDeathAnalytics()
        {
            _analyticsService.LogEntityDeath(_player.CurrentWeapon.BulletsFired);
        }

        private void CreateCancellationToken()
        {
            _cts = new CancellationTokenSource();
            _token = _cts.Token;
        }

        public void Dispose()
        {
            _player.OnDeath -= OnPlayerDeath;
            _cts?.Cancel();
            _cts?.Dispose();
        }
    }
}