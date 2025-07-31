using System;
using System.Collections;
using Project.Scripts.Animations.Enemy;
using Project.Scripts.Audio;
using Project.Scripts.GameFlowScripts;
using Project.Scripts.HealthInfo;
using Project.Scripts.Weapons;
using UnityEngine;

namespace Project.Scripts.Enemies
{
    public class EnemyModel : IPausable
    {
        public int EXP { get; private set; }
        private Weapon<StoneCannonConfig> CurrentWeapon { get; }
        public Health EnemyHealth { get; }
        private readonly AudioManager _audioManager;
        private readonly IEnemyAnimator _animator;
        private readonly MonoBehaviour _coroutineRunner;

        private bool _isDead;
        private Coroutine _attackRoutine;

        public event Action OnDeath;
        private Action _onDeathCallback;

        public EnemyModel(Weapon<StoneCannonConfig> weapon,
            Health health,
            int exp,
            AudioManager audioManager,
            IEnemyAnimator animator,
            MonoBehaviour coroutineRunner 
        )
        {
            CurrentWeapon = weapon;
            EnemyHealth = health;
            EXP = exp;
            _audioManager = audioManager;
            _animator = animator;
            _coroutineRunner = coroutineRunner;
        }
        
        public void SubscribeOnDeath(Action callback)
        {
            _onDeathCallback = callback;
            OnDeath += _onDeathCallback;
        }
        
        public void UnsubscribeFromDeath()
        {
            OnDeath -= _onDeathCallback;
            _onDeathCallback = null;
        }

        public void SubscribeToHealthChanged()
        {
            EnemyHealth.OnHealthChanged += OnHealthChanged;
        }

        public void UnsubscribeFromHealthChanged()
        {
            EnemyHealth.OnHealthChanged -= OnHealthChanged;
        }

        private void OnHealthChanged(float healthRatio)
        {
            if (_isDead) return;

            _animator?.PlayGetHit();

            if (EnemyHealth.IsDead)
            {
                _isDead = true;
                _audioManager.PlayEnemyDestroyedSound();
                
                _coroutineRunner.StartCoroutine(HandleDeath());
            }
        }

        private IEnumerator HandleDeath()
        {
            _animator?.PlayDie();
            yield return new WaitForSeconds(0.9f);

            OnDeath?.Invoke();
        }

        public void StartAutoAttack()
        {
            if (_attackRoutine == null && !_isDead)
                _attackRoutine = _coroutineRunner.StartCoroutine(AutoAttack());
        }

        public IEnumerator AutoAttack()
        {
            while (!_isDead)
            {
                Attack();
                yield return new WaitForSeconds(CurrentWeapon.Config.FireRate);
            }
        }

        private void Attack()
        {
            _animator?.PlayAttack();
            CurrentWeapon.InstantAttack();
        }

        public virtual void PauseAttack()
        {
            if (_attackRoutine != null)
            {
                _coroutineRunner.StopCoroutine(_attackRoutine);
                _attackRoutine = null;
            }
        }

        public virtual void ResumeAttack()
        {
            StartAutoAttack();
        }
    }
}
