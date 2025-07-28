using Project.Scripts.Audio;
using Project.Scripts.BulletModel;
using UnityEngine;

namespace Project.Scripts.Weapons
{
    public class Bow : Weapon<BowConfig>
    {
        private readonly Transform _bulletPosition;
        private readonly BulletFactoryPlayer _bulletFactory;
        private readonly AudioManager _audioManager;

        public Bow(BowConfig bowConfig, Transform bulletPosition, BulletFactoryPlayer bulletFactory, AudioManager audioManager)
            : base(bowConfig)
        {
            _bulletPosition = bulletPosition;
            _bulletFactory = bulletFactory;
            _audioManager = audioManager;
        }

        public override void InstantAttack()
        {
            var bullet = _bulletFactory.GetBullet(_bulletPosition.position, _bulletPosition.rotation);
            bullet.SetDamage(Config.Damage);
            bullet.Shoot(_bulletPosition.forward, Config.BulletSpeed);

            IncreaseBulletsFired();
            _audioManager.PlayShotSound();
        }
    }
}
