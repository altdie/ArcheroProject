using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using Project.Scripts.ADS;
using Project.Scripts.Enemies;
using Project.Scripts.Firebase;
using Project.Scripts.GameFlowScripts;
using Project.Scripts.PanelSettings;
using Project.Scripts.Players;
using Project.Scripts.SaveSystem;
using Project.Scripts.UI;
using UnityEngine;

public class PlayerLifecycle : IDisposable
{
    private int _killsCount;
        private bool _rewardAdsComplete;
        
        private PlayerModel _player;

        private readonly PlayerPrefsSave _playerPrefsSave;
        private readonly PlayerFactory _playerFactory;
        private readonly PlayerSpawnPoint _spawnPointPlayer;
        private readonly Joystick _joystick;
        private readonly IAnalyticsService _analyticsService;
        private readonly TimeService _timeService;
        private readonly SaveSelection _saveSelection;
        private readonly PanelFactory _panelFactory;
        private readonly SceneLoader _sceneLoader;
        private readonly AdsService _adsService;
        private readonly PlayerConfig _playerConfig;
        private PlayerStatsUIPresenter _playerStatsUIPresenter;
        private PlayerStatsUIModel _playerStatsUIModel;
        private CancellationTokenSource _cts;
        private CancellationToken _token;
        private PanelADSPresenter _panelPresenter;
        private readonly SceneData _sceneData;
        private readonly PlayerStatsUIView _playerStatsUIView;

        public PlayerLifecycle(
            PlayerFactory playerFactory,
            PlayerSpawnPoint spawnPointPlayer,
            Joystick joystick,
            IAnalyticsService analyticsService,
            TimeService timeService,
            SaveSelection saveSelection,
            PanelFactory panelFactory,
            SceneLoader sceneLoader, AdsService adsService, PlayerPrefsSave playerPrefsSave, 
            PlayerConfig playerConfig, SceneData sceneData, PlayerStatsUIPresenter playerStatsUIPresenter)
        {
            _playerFactory = playerFactory;
            _spawnPointPlayer = spawnPointPlayer;
            _joystick = joystick;
            _analyticsService = analyticsService;
            _timeService = timeService;
            _saveSelection = saveSelection;
            _panelFactory = panelFactory;
            _sceneLoader = sceneLoader;
            _adsService = adsService;
            _playerPrefsSave = playerPrefsSave;
            _playerConfig = playerConfig;
            _playerStatsUIPresenter = playerStatsUIPresenter;
            _sceneData = sceneData;
        }
      
        private async UniTask InitializePlayer()
        {
            CreateCancellationToken();
            _player = await _playerFactory.CreatePlayer(_spawnPointPlayer, _playerConfig.PlayerInitialHealth, _joystick);

            _playerStatsUIModel = new PlayerStatsUIModel(_player, _sceneData.MaxExperience);
            _playerStatsUIPresenter = new PlayerStatsUIPresenter(_playerStatsUIModel, _playerStatsUIView);
            _playerStatsUIPresenter.Initialize();
            _player.OnDeath += OnPlayerDeath;
            var loadDataTask = LoadPlayerData(_token);
            await UniTask.WhenAll(loadDataTask);
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
                await LoadPlayerData(_token);
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

    private async UniTask RevivePlayer()
    {
        _rewardAdsComplete = true;
        _playerPrefsSave.Load();
        _player = await _playerFactory.CreatePlayer(_spawnPointPlayer, _playerConfig.PlayerInitialHealth, _joystick);
        _player.OnDeath += OnPlayerDeath;
        _panelFactory.DestroyPanel();
        _timeService.ResumeAttack();
    }
    
    private async UniTask LoadPlayerData(CancellationToken token)
    {
        token.ThrowIfCancellationRequested();
        PlayerDataSave savedData = await _saveSelection.Load(token);
        token.ThrowIfCancellationRequested();
        _player.Experience = savedData.Experience;
        _player.IsAdsRemoved = savedData.IsAdsRemoved;
    }
    
    private async UniTask ClearData(CancellationToken token)
    {
        await _saveSelection.Clear(token);
    }

    private void LogDeathAnalytics()
    {
        _analyticsService.LogEntityDeath(_player.CurrentWeapon.BulletsFired);
    }
    
    public void Dispose()
    {
        _player.OnDeath -= OnPlayerDeath;
        _player.UnsubscribeFromHealthChanged();
        _cts?.Cancel();
        _cts?.Dispose();
    }
}
