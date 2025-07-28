using System;
using System.Threading.Tasks;
using Project.Scripts.Animations.Character;
using Project.Scripts.GameFlowScripts;
using Project.Scripts.HealthInfo;
using Project.Scripts.Players;
using Project.Scripts.Weapons;
using Zenject;

namespace Project.Scripts.PlayerModels
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
        public int Speed = 5; // test 
        private bool _isAttacking;

        public Health PlayerHealth { get; private set; }
        public Weapon<BowConfig> CurrentWeapon;
        public PlayerMovement PlayerMovement { get; }
        private readonly Joystick _joystick;
        private readonly ICharacterAnimator _animator;
        public event Action OnDeath;
        public event Action OnExperienceChanged;

        public PlayerModel(Health playerHealth, int speed, Weapon<BowConfig> currentWeapon, PlayerMovement playerMovement, 
            Joystick joystick, int experience, int level, bool isAdsRemoved, int lastSave, ICharacterAnimator animator)
        {
            PlayerHealth = playerHealth;
            Speed = speed;
            CurrentWeapon = currentWeapon;
            PlayerMovement = playerMovement;
            _joystick = joystick;
            Experience = experience;
            Level = level;
            IsAdsRemoved = isAdsRemoved;
            LastSave = lastSave;
            _animator = animator;
        }

        public void SubscribeOnHealthChanged()
        {
            PlayerHealth.OnHealthChanged += OnHealthChanged;
        }

        public void Move()
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
                _animator?.PlayAttack();
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
        
        public void PlayWalk() => _animator?.PlayWalk();
        public void StopWalk() => _animator?.StopWalk();

        private void OnHealthChanged(float healthRatio)
        {
            if (PlayerHealth.IsDead)
            {
                OnDeath?.Invoke();
            }
        }
    }
}