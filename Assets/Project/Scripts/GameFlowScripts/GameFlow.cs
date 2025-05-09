using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Project.Scripts.ADS;
using Project.Scripts.Enemies;
using Project.Scripts.Firebase;
using Project.Scripts.Installers;
using Project.Scripts.PanelSettings;
using Project.Scripts.PlayerModels;
using Project.Scripts.Players;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Project.Scripts.GameFlowScripts
{
    public class GameFlow : IInitializable, ITickable, IDisposable
    {
        private int _killsCount;
        private int _levelCount;
        private bool _rewardAdsComplete = false;

        private List<EnemyModel> _enemies;
        private PlayerModel _player;

        private readonly EnemyFactory _enemyFactory;
        private readonly PlayerFactory _playerFactory;
        private readonly PlayerSpawnPoint _spawnPointPlayer;
        private readonly Joystick _joystick;
        private readonly PanelModel _panelModel;
        private readonly PanelPresenter _panelPresenter;
        private readonly EnemySpawnData[] _enemySpawnData;
        private readonly Slider _experienceSlider;
        private readonly SceneData _sceneData;
        private readonly IAnalyticsService _analyticsService;
        private readonly TextMeshProUGUI _levelText;
        private readonly AdsInitializer _adsInitializer;
        private readonly InterstitialAds _interstitialAdExample;
        private readonly RewardedAds _rewardedAds;
        private readonly TimeService _timeService;
        private readonly SaveSelection _saveSelection;

        public GameFlow(
            EnemyFactory enemyFactory,
            PlayerFactory playerFactory,
            PlayerSpawnPoint spawnPointPlayer,
            Joystick joystick,
            PanelModel panelModel,
            PanelPresenter panelPresenter,
            EnemySpawnData[] enemySpawnData,
            Slider experienceSlider,
            SceneData sceneData,
            IAnalyticsService analyticsService,
            TextMeshProUGUI textMeshProUGUI,
            AdsInitializer adsInitializer,
            InterstitialAds interstitialAdExample,
            RewardedAds rewardedAds,
            TimeService timeService,
            SaveSelection saveSelection)
        {
            _enemyFactory = enemyFactory;
            _playerFactory = playerFactory;
            _spawnPointPlayer = spawnPointPlayer;
            _joystick = joystick;
            _panelModel = panelModel;
            _panelPresenter = panelPresenter;
            _enemySpawnData = enemySpawnData;
            _experienceSlider = experienceSlider;
            _sceneData = sceneData;
            _analyticsService = analyticsService;
            _levelText = textMeshProUGUI;
            _adsInitializer = adsInitializer;
            _interstitialAdExample = interstitialAdExample;
            _rewardedAds = rewardedAds;
            _timeService = timeService;
            _saveSelection = saveSelection;
        }

        public void Initialize()
        {
            _panelModel.DisablePanels();
            _ = InitializeAsync();
            _adsInitializer.InitializeAds();
            _interstitialAdExample.Initialize();
            _panelPresenter.OnRewardedAdClicked += RevivePlayer;
            _rewardedAds.OnAdWatched += RevivePlayer;
            _rewardAdsComplete = false;
        }

        private async Task InitializeAsync()
        {
            _player = await _playerFactory.CreatePlayerAsync(_spawnPointPlayer, 100, _joystick);
            _enemyFactory.CreateEnemies(_enemySpawnData);
            _enemies = _enemyFactory.Enemies;

            _player.PlayerHealth.OnEntityDeath += RemovePlayer;

            SetupEnemies();
            await LoadPlayerDataAsync();
            UpdateExperienceSlider();
        }

        private Action GetEnemyDeathHandler(EnemyModel enemy)
        {
            return () => RemoveEnemy(enemy);
        }

        private async Task LoadPlayerDataAsync()
        {
            PlayerDataSave savedData = await _saveSelection.LoadAsync();
            _player.Experience = savedData.Experience;
            _levelCount = savedData.Level;
            _player.IsAdsRemoved = savedData.IsAdsRemoved;
            _levelText.text = "Level: " + _levelCount;
        }

        private async void RemoveEnemy(EnemyModel enemy)
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
            _panelModel.EnableCollider();
            LevelUp();
        }

        private void RemovePlayer()
        {
            _player.PlayerHealth.OnEntityDeath -= RemovePlayer;
            _timeService.Pause();

            if (!_rewardAdsComplete)
            {
                _ = _panelModel.CreatePanelAsync();
            }
            else
            {
                _ = _panelModel.CreatePanelAsync();
                _ =_saveSelection.ClearAsync();
                _ = LoadPlayerDataAsync();
                UpdateExperienceSlider();
                LogDeathAnalytics();
            }
        }

        private async void RevivePlayer()
        {
            _panelPresenter.OnRewardedAdClicked -= RevivePlayer;
            _rewardAdsComplete = true;

            _panelModel.DisablePanels();
            _player = await _playerFactory.CreatePlayerAsync(_spawnPointPlayer, 100, _joystick);
            _player.PlayerHealth.OnEntityDeath += RemovePlayer;
            _timeService.SetPlayerModel(_player);
            _timeService.Continue();
        }

        private void UpdateExperienceSlider()
        {
            float current = _player.Experience;
            float max = _sceneData.MaxExperience;
            _experienceSlider.value = Mathf.Clamp(current / max, 0f, 1f);
        }

        private void LevelUp()
        {
            _levelCount++;
            _analyticsService.LogLevelPassed(_levelCount);
            _levelText.text = "Level: " + _levelCount;
            _ = _saveSelection.SaveAsync(_player);
        }

        public async void ClearData()
        {
            await _saveSelection.ClearAsync();
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
                _timeService.SetPEnemyModel(enemy);
            }
        }

        public void Tick()
        {
            _player?.Move();
        }

        public void Dispose()
        {
            _panelPresenter.OnRewardedAdClicked -= RevivePlayer;
            _rewardedAds.OnAdWatched -= RevivePlayer;
        }
    }
}
