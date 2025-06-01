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
using Project.Scripts.SaveSystem;
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
        private readonly EnemySpawnData[] _enemySpawnData;
        private readonly Slider _experienceSlider;
        private readonly SceneData _sceneData;
        private readonly IAnalyticsService _analyticsService;
        private readonly TextMeshProUGUI _levelText;
        private readonly AdsInitializer _adsInitializer;
        private readonly InterstitialAds _interstitialAdExample;
        private readonly TimeService _timeService;
        private readonly SaveSelection _saveSelection;
        private readonly PanelFactory _panelFactory;
        private readonly DoorView _doorView;

        public GameFlow(
            EnemyFactory enemyFactory,
            PlayerFactory playerFactory,
            PlayerSpawnPoint spawnPointPlayer,
            Joystick joystick,
            EnemySpawnData[] enemySpawnData,
            Slider experienceSlider,
            SceneData sceneData,
            IAnalyticsService analyticsService,
            TextMeshProUGUI textMeshProUGUI,
            AdsInitializer adsInitializer,
            InterstitialAds interstitialAdExample,
            TimeService timeService,
            SaveSelection saveSelection,
            PanelFactory panelFactory,
            DoorView doorView)
        {
            _enemyFactory = enemyFactory;
            _playerFactory = playerFactory;
            _spawnPointPlayer = spawnPointPlayer;
            _joystick = joystick;
            _enemySpawnData = enemySpawnData;
            _experienceSlider = experienceSlider;
            _sceneData = sceneData;
            _analyticsService = analyticsService;
            _levelText = textMeshProUGUI;
            _adsInitializer = adsInitializer;
            _interstitialAdExample = interstitialAdExample;
            _timeService = timeService;
            _saveSelection = saveSelection;
            _panelFactory = panelFactory;
            _doorView = doorView;
        }

        public void Initialize()
        {
            _ = InitializeAsync();
            _adsInitializer.InitializeAds();
            _interstitialAdExample.Initialize();
            _timeService.OnPause += HandleOnPause;
            _timeService.OnResume += HandleOnResume;
            _rewardAdsComplete = false;
            _doorView.Disable();
        }

        public void ShowRewardedAdCallback()
        {
           //ShowRewardedAd(RevivePlayer);
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
            _doorView.Enable();
            LevelUp();
        }

        private void RemovePlayer()
        {
            _player.PlayerHealth.OnEntityDeath -= RemovePlayer;
            _timeService.PauseAttack();

            if (!_rewardAdsComplete)
            {
                _ = _panelFactory.CreatePanelAsync();
            }
            else
            {
                _ = _panelFactory.CreatePanelAsync();
                _ = _saveSelection.ClearAsync();
                _ = LoadPlayerDataAsync();
                UpdateExperienceSlider();
                LogDeathAnalytics();
            }
        }

        public async void RevivePlayer()
        {
            _rewardAdsComplete = true;

            _player = await _playerFactory.CreatePlayerAsync(_spawnPointPlayer, 100, _joystick);
            _player.PlayerHealth.OnEntityDeath += RemovePlayer;
            _timeService.ResumeAttack();
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
            }
        }

        private void HandleOnPause()
        {
            _player?.PauseAttack();

            foreach (var enemy in _enemies)
            {
                if (enemy is IPausable pausableEnemy)
                {
                    pausableEnemy.PauseAttack();
                }
            }
        }

        private void HandleOnResume()
        {
            _player?.ResumeAttack();

            foreach (var enemy in _enemies)
            {
                if (enemy is IPausable pausableEnemy)
                {
                    pausableEnemy.ResumeAttack();
                }
            }
        }

        public void Tick()
        {
            _player?.Move();
        }

        public void Dispose()
        {
            
        }
    }
}