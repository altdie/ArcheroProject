using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using Project.Scripts.ADS;
using Project.Scripts.Enemies;
using Project.Scripts.Firebase;
using Project.Scripts.Installers;
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

        private readonly EnemyFactory _enemyFactory;
        private readonly PlayerFactory _playerFactory;
        private readonly PlayerSpawnPoint _spawnPointPlayer;
        private readonly Joystick _joystick;
        private readonly EnemySpawnData[] _enemySpawnData;
        private readonly SceneData _sceneData;
        private readonly IAnalyticsService _analyticsService;
        private readonly AdsInitializer _adsInitializer;
        private readonly InterstitialAds _interstitialAdExample;
        private readonly TimeService _timeService;
        private readonly SaveSelection _saveSelection;
        private readonly PanelFactory _panelFactory;
        private readonly DoorView _doorView;
        private readonly SceneLoader _sceneLoader;
        private PlayerStatsUIPresenter _playerStatsUIPresenter;
        private PlayerStatsUIModel _playerStatsUIModel;
        private readonly PlayerStatsUIView _playerStatsUIView;
        private CancellationTokenSource _cts;
        private CancellationToken _token;
        private PanelPresenter _panelPresenter;
        private readonly AdsService _adsService;

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
            DoorView doorView,PlayerStatsUIView playerStatsUIView, SceneLoader sceneLoader, AdsService adsService)
        {
            _enemyFactory = enemyFactory;
            _playerFactory = playerFactory;
            _spawnPointPlayer = spawnPointPlayer;
            _joystick = joystick;
            _enemySpawnData = enemySpawnData;
            _sceneData = sceneData;
            _analyticsService = analyticsService;
            _adsInitializer = adsInitializer;
            _interstitialAdExample = interstitialAdExample;
            _timeService = timeService;
            _saveSelection = saveSelection;
            _panelFactory = panelFactory;
            _doorView = doorView;
            _playerStatsUIView = playerStatsUIView;
            _sceneLoader = sceneLoader;
            _adsService = adsService;
        }

        public async void Initialize()
        {
            await InitializeAsync();
            _adsInitializer.InitializeAds();
            _adsService.LoadInterstitialAd();
            _interstitialAdExample.Initialize();
            _rewardAdsComplete = false;
            _doorView.Disable();
        }

        public void ShowRewardedAdCallback()
        {
            _adsService.ShowRewardedAd(() => RevivePlayer().Forget());
        }

        private async UniTask InitializeAsync()
        {
            CreateCancellationToken();
            _player = await _playerFactory.CreatePlayerAsync(_spawnPointPlayer, 100, _joystick);
            _enemyFactory.CreateEnemies(_enemySpawnData);
            _enemies = _enemyFactory.Enemies;
            _playerStatsUIModel = new PlayerStatsUIModel(_player, _sceneData.MaxExperience);
            _playerStatsUIPresenter = new PlayerStatsUIPresenter(_playerStatsUIModel, _playerStatsUIView);
            _player.PlayerHealth.OnEntityDeath += OnPlayerDeath;

            SetupEnemies();
            await LoadPlayerDataAsync(_token);
            UpdateExperienceSlider();
        }

        private Action GetEnemyDeathHandler(EnemyModel enemy)
        {
            return () => _ = RemoveEnemy(enemy);
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
            enemy.EnemyHealth.OnEntityDeath -= GetEnemyDeathHandler(enemy);
            _enemies.Remove(enemy);
            _player.PlayerMovement.AddExperience(enemy.EXP);
            UpdateExperienceSlider();

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
            PanelView panelView = await _panelFactory.CreatePanelAsync(_token);
            _panelPresenter = new PanelPresenter(panelView, this, _sceneLoader);

            if (!_rewardAdsComplete)
            {
                await ClearData(_token);
            }
            else
            {
                await _saveSelection.ClearAsync(_token);
                await LoadPlayerDataAsync(_token);
                UpdateExperienceSlider();
                LogDeathAnalytics();
            }
        }

        private void OnPlayerDeath()
        {
            _ = RemovePlayer();
        }

        public async UniTask RevivePlayer()
        {
            _rewardAdsComplete = true;

            _player = await _playerFactory.CreatePlayerAsync(_spawnPointPlayer, 100, _joystick);
            _player.PlayerHealth.OnEntityDeath += OnPlayerDeath;
            _panelFactory.DestroyPanel();
            _timeService.ResumeAttack();
        }

        private void UpdateExperienceSlider()
        {
            _playerStatsUIPresenter.UpdateView();
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

        private void SetupEnemies()
        {
            foreach (var enemy in _enemies)
            {
                enemy.EnemyHealth.OnEntityDeath += GetEnemyDeathHandler(enemy);
            }
        }

        private void CreateCancellationToken()
        {
            _cts = new CancellationTokenSource();
            _token = _cts.Token;
        }

        public void Dispose()
        {
            _player.PlayerHealth.OnEntityDeath -= OnPlayerDeath;
            _cts?.Cancel();
            _cts?.Dispose();
        }
    }
}