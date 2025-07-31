using Project.Scripts.Animations.Enemy;
using Project.Scripts.Audio;
using Project.Scripts.HealthInfo;
using Project.Scripts.Weapons;
using UnityEngine;

namespace Project.Scripts.Enemies
{
    public class StoneEnemy : EnemyModel
    {
        private readonly MonoBehaviour _coroutineRunner;
        private Coroutine _attackCoroutine;

        public StoneEnemy(EnemyStoneConfig config, SceneData coroutineRunner, Weapon<StoneCannonConfig> weapon, Health health, 
            AudioManager audioManager, IEnemyAnimator animator)
            : base(weapon, health, config.EXP, audioManager, animator, coroutineRunner )
        {
            _coroutineRunner = coroutineRunner;
            StartAttack();
        }

        private void StartAttack()
        {
            if (_attackCoroutine == null)
                _attackCoroutine = _coroutineRunner.StartCoroutine(AutoAttack());
        }

        private void StopAttack()
        {
            if (_attackCoroutine == null)
                return;

            _coroutineRunner.StopCoroutine(_attackCoroutine);
            _attackCoroutine = null;
        }
    }
}
