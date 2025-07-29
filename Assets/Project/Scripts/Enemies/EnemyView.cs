using System;
using Project.Scripts.HealthInfo;
using Project.Scripts.VFX;
using UnityEngine;
using UnityEngine.UI;

namespace Project.Scripts.Enemies
{
    public class EnemyView : MonoBehaviour
    {
        public Transform[] WeaponTransform;
        [SerializeField] private Slider _healthBar;
        [SerializeField] private VFXSpawner _vfxSpawner;

        private EnemyModel _enemyModel;
        private Health _health;

        public void Initialize(EnemyModel enemyModel, Transform[] weaponTransform, Health health)
        {
            _enemyModel = enemyModel;
            WeaponTransform = weaponTransform;
            _health = health;
            _healthBar.maxValue = 1f;
            _healthBar.value = _health.CurrentHealth / _health.MaxHealth;
        }

        public void Subscribe()
        {
            _health.OnHealthChanged += UpdateHealthBar;
            _enemyModel.OnDeath += Die;
        }

        private void Unsubscribe()
        {
            _health.OnHealthChanged -= UpdateHealthBar;
            _enemyModel.OnDeath -= Die;
        }

        private void OnDestroy()
        {
            Unsubscribe();
        }

        public void TakeDamage(float damage)
        {
            _enemyModel.EnemyHealth.TakeDamage(damage);
        }

        private void UpdateHealthBar(float currentHealthRatio)
        {
            _healthBar.value = currentHealthRatio;
        }

        private void Die()
        {
            _vfxSpawner.SpawnEffect(transform.position);
            Destroy(gameObject);
        }
    }
}
