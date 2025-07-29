using Project.Scripts.Enemies;
using Project.Scripts.HealthInfo;
using Project.Scripts.Weapons;
using UnityEngine;
using Project.Scripts.Addressables;
using Project.Scripts.GameFlowScripts;
using Zenject;
using Cysharp.Threading.Tasks;
using Project.Scripts.Animations.Character;

namespace Project.Scripts.Players
{
    public class PlayerFactory
    {
        private readonly WeaponFactory _weaponFactory;
        private readonly SceneData _sceneData;
        private readonly IAssetProvider _assetProvider;
        private readonly PlayerPrefsSave _playerPrefsSave;
        private readonly DiContainer _container;

        public PlayerFactory(
            WeaponFactory weaponFactory,
            SceneData sceneData,
            IAssetProvider assetProvider,
            PlayerPrefsSave playerPrefsSave,
            DiContainer container)
        {
            _weaponFactory = weaponFactory;
            _sceneData = sceneData;
            _assetProvider = assetProvider;
            _playerPrefsSave = playerPrefsSave;
            _container = container;
        }

        public async UniTask<PlayerModel> CreatePlayerAsync(PlayerSpawnPoint spawnPosition, int initialHealth, Joystick joystick)
        {
            GameObject playerPrefab = await _assetProvider.CreatePlayerPrefabAsync();
            GameObject playerObj = Object.Instantiate(playerPrefab, spawnPosition.transform.position, Quaternion.identity);
            PlayerMovement playerMovement = playerObj.GetComponent<PlayerMovement>();
            
            var playerInput = new PlayerInputHandler(joystick);
            var weapon = _weaponFactory.CreateWeapon(playerMovement.weaponTransformPrefab);
            var health = new Health(initialHealth, playerMovement.gameObject);
            var playerSaveData = _playerPrefsSave.Load();
            var player = new PlayerModel(
                health,
                10,
                weapon,
                playerMovement,
                playerInput.Joystick,
                playerSaveData.Experience,
                playerSaveData.Level,
                playerSaveData.IsAdsRemoved,
                playerSaveData.LastSaved);

            player.SubscribeOnHealthChanged();
            
            PlayerView playerView = playerMovement.GetComponent<PlayerView>();
            EntityAnimatorProvider animatorProvider = playerObj.GetComponentInChildren<EntityAnimatorProvider>();
            ICharacterAnimator characterAnimator = new CharacterAnimator(animatorProvider.Animator);
            playerView.Initialize(player, characterAnimator);
            playerView.SubscribeToModel();
            
            playerMovement.Initialize(player, playerInput, health, _sceneData, playerSaveData.Experience);
            playerMovement.SetupHealthUI();
            playerMovement.Subscribe();
            
            player.SetWeapon(weapon);
            
            _container.Inject(player);
            _container.Resolve<TickableManager>().Add(player);

            return player;
        }
    }
}
