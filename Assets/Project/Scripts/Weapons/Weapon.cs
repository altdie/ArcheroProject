namespace Project.Scripts.Weapons
{
    public abstract class Weapon<T> where T : WeaponConfig
    {
        public T Config { get; }
        public int BulletsFired { get; private set; }


        protected Weapon(T config)
        {
            Config = config;
        }

        public abstract void InstantAttack();

        protected void IncreaseBulletsFired()
        {
            BulletsFired++;
        }
    }
}