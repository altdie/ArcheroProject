using System;
using Project.Scripts.HealthInfo;
using UnityEngine;
using UnityEngine.UI;

namespace Project.Scripts.Enemies
{
    public class EnemyView : MonoBehaviour
    {
        public event Action OnEntityDeath;

        public Transform[] WeaponTransform;
        [SerializeField] private Slider _healthBar;

        private EnemyModel _enemyModel;

        public void Initialize(EnemyModel enemyModel, Transform[] weaponTransform, Health health)
        {
            _enemyModel = enemyModel;
            WeaponTransform = weaponTransform;

            _healthBar.maxValue = 1f;
            _healthBar.value = health.CurrentHealth / health.MaxHealth;

            health.OnHealthChanged += UpdateHealthBar;
            _enemyModel.OnDeath += Die;
        }

        private void OnDestroy()
        {
            if (_enemyModel != null)
                _enemyModel.OnDeath -= Die;
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
            OnEntityDeath?.Invoke();
            Destroy(gameObject);
        }
    }
}
