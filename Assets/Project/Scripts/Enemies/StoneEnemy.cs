using Project.Scripts.Enemies;
using Project.Scripts.GameFlowScripts;
using Project.Scripts.HealthInfo;
using Project.Scripts.Weapons;
using UnityEngine;

namespace Project.Scripts.Enemy
{
    public class StoneEnemy : EnemyModel, IPausable
    {
        private readonly MonoBehaviour _coroutineRunner;
        private Coroutine _attackCoroutine;

        public StoneEnemy(EnemyStoneConfig config, SceneData coroutineRunner, Weapon<StoneCannonConfig> weapon, Health health)
            : base(config, weapon, health, config.EXP)
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

        public override void PauseAttack()
        {
            StopAttack();
        }

        public override void ResumeAttack()
        {
            StartAttack();
        }
    }
}
