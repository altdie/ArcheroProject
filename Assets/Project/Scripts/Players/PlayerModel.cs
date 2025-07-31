using System;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using Project.Scripts.Animations.Character;
using Project.Scripts.GameFlowScripts;
using Project.Scripts.HealthInfo;
using Project.Scripts.Weapons;
using Zenject;

namespace Project.Scripts.Players
{
    public class PlayerModel : IPausable, ITickable
    {
        private const int ATTACK_DELAY = 250;

        private int _experience;
        public int Experience
        {
            get => _experience;
            set
            {
                if (_experience != value)
                {
                    _experience = value;
                    OnExperienceChanged?.Invoke();
                }
            }
        }
        public int Level { get; set; }
        public bool IsAdsRemoved { get; set; }
        public int LastSave { get; set; }
        public readonly int Speed;
        private bool _isAttacking;

        public Health PlayerHealth { get; private set; }
        public Weapon<BowConfig> CurrentWeapon;
        public PlayerMovement PlayerMovement { get; }
        public event Action OnDeath;
        public event Action OnExperienceChanged;
        public event Action OnAttack;
        public event Action OnWalk;
        public event Action OnStopWalk;

        public PlayerModel(Health playerHealth, int speed, Weapon<BowConfig> currentWeapon, PlayerMovement playerMovement, 
            int experience, int level, bool isAdsRemoved, int lastSave)
        {
            PlayerHealth = playerHealth;
            Speed = speed;
            CurrentWeapon = currentWeapon;
            PlayerMovement = playerMovement;
            Experience = experience;
            Level = level;
            IsAdsRemoved = isAdsRemoved;
            LastSave = lastSave;
        }

        public void SubscribeOnHealthChanged()
        {
            PlayerHealth.OnHealthChanged += OnHealthChanged;
        }
        
        public void UnsubscribeFromHealthChanged()
        {
            PlayerHealth.OnHealthChanged -= OnHealthChanged;
        }


        private void Move()
        {
            PlayerMovement.Move();
        }

        public void StopAttacking()
        {
            _isAttacking = false;
        }

        public async void StartAttack()
        {
            if (_isAttacking)
            {
                return;
            }

            _isAttacking = true;

            while (_isAttacking)
            {
                CurrentWeapon.InstantAttack();
                await Task.Delay(ATTACK_DELAY);
            }
        }

        public void SetWeapon(Weapon<BowConfig> weapon)
        {
            CurrentWeapon = weapon;
        }

        public void PauseAttack()
        {
            StopAttacking();
        }

        public void ResumeAttack()
        {
            StartAttack();
        }

        public void Tick()
        {
            Move();
        }
        
        private void OnHealthChanged(float healthRatio)
        {
            if (PlayerHealth.IsDead)
            {
                OnDeath?.Invoke();
            }
        }
        
        public void TriggerAttack() => OnAttack?.Invoke();
        public void TriggerWalk() => OnWalk?.Invoke();
        public void TriggerStopWalk() => OnStopWalk?.Invoke();
    }
}