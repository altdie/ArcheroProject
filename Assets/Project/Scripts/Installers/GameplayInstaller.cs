using System.Collections.Generic;
using Assets.Project.Scripts.ADS;
using Project.Scripts.Addressables;
using Project.Scripts.ADS;
using Project.Scripts.BulletFactoryEnemy;
using Project.Scripts.BulletModel;
using Project.Scripts.Enemies;
using Project.Scripts.Firebase;
using Project.Scripts.GameFlowScripts;
using Project.Scripts.PanelSettings;
using Project.Scripts.Players;
using Project.Scripts.SaveSystem;
using Project.Scripts.UI;
using Project.Scripts.Weapons;
using UnityEngine;
using Zenject;

namespace Project.Scripts.Installers
{
    public class GameplayInstaller : MonoInstaller
    {
        [SerializeField] private PlayerSpawnPoint _spawnPointPlayer;
        [SerializeField] private Joystick _joystick;
        [SerializeField] private BowConfig _bowConfig;
        [SerializeField] private StoneCannonConfig _stoneCannonConfig;
        [SerializeField] private EnemySpawnData[] _enemySpawnData;
        [SerializeField] private SceneData _sceneData;
        [SerializeField] private Collider _levelCollider;
        [SerializeField] private PanelView _panelGameOver;
        [SerializeField] private Canvas _panelCanvas;
        [SerializeField] private PlayerStatsUIView _playerStatsUIView;

        public override void InstallBindings()
        {
            BindInstances();
            BindServices();
            BindFactories();
            BindConfigs();
            BindUI();
            BindGameLogic();
        }

        private void BindFactories()
        {
            Container.Bind<WeaponFactory>().AsSingle();
            Container.Bind<PlayerFactory>().AsSingle();
            Container.Bind<PanelFactory>().AsSingle();
            Container.Bind<EnemyFactory>().AsSingle().WithArguments(_sceneData);
            Container.Bind<BulletFactoryEnemies>().AsSingle().WithArguments(_stoneCannonConfig);
            Container.Bind<BulletFactoryPlayer>().AsSingle().WithArguments(_bowConfig);
        }

        private void BindConfigs()
        {
            Container.Bind<BowConfig>().FromInstance(_bowConfig).AsSingle();
            Container.Bind<StoneCannonConfig>().FromInstance(_stoneCannonConfig).AsSingle();
        }

        private void BindInstances()
        {
            Container.BindInstance(_spawnPointPlayer).AsSingle();
            Container.BindInstance(_joystick).AsSingle();
            Container.BindInstance(_enemySpawnData).AsSingle();
            Container.BindInstance(_sceneData).AsSingle();
            Container.BindInstance(_playerStatsUIView).AsSingle();
        }

        private void BindUI()
        {
            Container.Bind<AdsInitializer>().AsSingle();
            Container.Bind<InterstitialAds>().AsSingle();
            Container.Bind<RewardedAds>().AsSingle();
            Container.Bind<IAds>().To<AdsService>().AsSingle();
            Container.BindInstance(_panelCanvas).AsSingle();
            Container.BindInstance(_levelCollider).AsSingle();
            Container.Bind<PanelView>()
               .FromComponentInNewPrefab(_panelGameOver)
               .AsSingle()
               .NonLazy();
            Container.Bind<PlayerStatsUIModel>().AsSingle();
            Container.Bind<PlayerStatsUIPresenter>().AsSingle().WithArguments(_playerStatsUIView);
        }

        private void BindServices()
        {
            Container.Bind<IAssetProvider>().To<AssetProvider>().AsSingle();
            Container.Bind<IAnalyticsService>().To<FirebaseAnalyticsService>().AsSingle();
            Container.Bind<PlayerDataSave>().AsSingle();
            Container.Bind<PlayerPrefsSave>().AsSingle();
            Container.Bind<TimeService>().AsSingle();
            Container.Bind<CloudSave>().AsSingle();
            Container.Bind<SaveSelection>().AsSingle();
        }

        private void BindGameLogic()
        {
            Container.BindInterfacesAndSelfTo<GameFlow>().AsSingle();
            Container.Bind<List<IPausable>>().AsSingle();
            Container.Bind<DoorView>().AsSingle().WithArguments(_levelCollider);
            Container.Bind<SceneLoader>().AsSingle();
        }
    }
}
