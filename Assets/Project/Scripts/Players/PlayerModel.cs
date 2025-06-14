using System.Threading.Tasks;
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

        public int Experience { get; set; }
        public int Level { get; set; }
        public bool IsAdsRemoved { get; set; }
        public int LastSave { get; set; }
        public int Speed = 5; // test 
        private bool _isAttacking;

        public Health PlayerHealth { get; private set; }
        public Weapon<BowConfig> CurrentWeapon;
        public PlayerMovement PlayerMovement { get; }
        private readonly Joystick _joystick;

        public PlayerModel(Health playerHealth, int speed, Weapon<BowConfig> currentWeapon, PlayerMovement playerMovement, Joystick joystick, int experience, int level, bool isAdsRemoved, int lastSave)
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
    }
}