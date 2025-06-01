using System.Collections;
using Project.Scripts.GameFlowScripts;
using Project.Scripts.HealthInfo;
using Project.Scripts.Weapons;
using UnityEngine;

namespace Project.Scripts.Enemies
{
    public class EnemyModel : IPausable
    {
        public int EXP { get; private set; }
        private Weapon<StoneCannonConfig> CurrentWeapon { get; set; }
        public Health EnemyHealth { get; private set; }

        public EnemyModel(EnemyConfig config, Weapon<StoneCannonConfig> weapon, Health health, int eXP)
        {
            CurrentWeapon = weapon;
            EnemyHealth = health;
            EXP = eXP;
        }

        private void Attack()
        {
            CurrentWeapon.InstantAttack();
        }
        
        public IEnumerator AutoAttack()
        {
            while (true)
            {
                Attack();
                yield return new WaitForSeconds(CurrentWeapon.Config.FireRate);
            }
        }

        public virtual void PauseAttack()
        {
            // по умолчанию ничего не делаем
        }

        public virtual void ResumeAttack()
        {
            // по умолчанию ничего не делаем
        }
    }
}