using Project.Scripts.BulletFactoryEnemy;
using Project.Scripts.BulletModel;
using UnityEngine;
using Zenject;

namespace Project.Scripts.Weapons
{
    public class WeaponFactory
    {
        private BowConfig _bowConfig;
        private BulletFactoryPlayer _bulletFactory;
        private StoneCannonConfig _stoneCannonConfig;
        private BulletFactoryEnemies _bulletFactoryEnemy;
        private AudioManager _audioManager;

        [Inject]
        public void Construct(BowConfig bowConfig, BulletFactoryPlayer bulletFactory,
            StoneCannonConfig stoneCannonConfig, BulletFactoryEnemies bulletFactoryEnemy, AudioManager audioManager)
        {
            _bowConfig = bowConfig;
            _bulletFactory = bulletFactory;
            _stoneCannonConfig = stoneCannonConfig;
            _bulletFactoryEnemy = bulletFactoryEnemy;
            _audioManager = audioManager;
        }

        public Bow CreateWeapon(Transform spawnPoint)
        {
            return new Bow(_bowConfig, spawnPoint, _bulletFactory, _audioManager);
        }

        public StoneCannon CreateEnemyWeapon(Transform[] spawnPoints)
        {
            return new StoneCannon(_stoneCannonConfig, spawnPoints, _bulletFactoryEnemy);
        }
    }
}
