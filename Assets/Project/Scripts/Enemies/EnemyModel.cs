using System;
using System.Collections;
using Project.Scripts.GameFlowScripts;
using Project.Scripts.HealthInfo;
using Project.Scripts.Weapons;

namespace Project.Scripts.Enemies
{
    public class EnemyModel : IPausable
    {
        public int EXP { get; private set; }
        private Weapon<StoneCannonConfig> CurrentWeapon { get; set; }
        public Health EnemyHealth { get; private set; }
        private readonly AudioManager _audioManager;

        public event Action OnDeath;

        public EnemyModel(EnemyConfig config, Weapon<StoneCannonConfig> weapon, Health health, int exp, AudioManager audioManager)
        {
            CurrentWeapon = weapon;
            EnemyHealth = health;
            EXP = exp;
            Subscribe();
            _audioManager = audioManager;
        }

        private void Subscribe() // спросить у Миши
        {
            EnemyHealth.OnHealthChanged += OnHealthChanged;
        }

        private void OnHealthChanged(float healthRatio)
        {
            if (EnemyHealth.IsDead)
            {
                _audioManager.PlayEnemyDestroyedSound();
                OnDeath?.Invoke();
            }
        }

        public IEnumerator AutoAttack()
        {
            while (true)
            {
                Attack();
                yield return new UnityEngine.WaitForSeconds(CurrentWeapon.Config.FireRate);
            }
        }

        private void Attack()
        {
            CurrentWeapon.InstantAttack();
        }

        public virtual void PauseAttack() { }

        public virtual void ResumeAttack() { }
    }
}
