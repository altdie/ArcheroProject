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
        private readonly AudioManager _audioManager;

        public StoneEnemy(EnemyStoneConfig config, SceneData coroutineRunner, Weapon<StoneCannonConfig> weapon, Health health, AudioManager audioManager)
            : base(config, weapon, health, config.EXP, audioManager)
        {
            _coroutineRunner = coroutineRunner;
            StartAttack();
            _audioManager = audioManager;
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
