using System;
using UnityEngine;

namespace Project.Scripts.HealthInfo
{
    public class Health
    {
        public event Action<float> OnHealthChanged;
        public float MaxHealth { get; private set; }
        public float CurrentHealth { get; private set; }
        private readonly GameObject _entityObject;

        public Health(float maxHealth, GameObject entityObject)
        {
            MaxHealth = maxHealth;
            CurrentHealth = maxHealth;
            _entityObject = entityObject;
        }

        public void TakeDamage(float damage)
        {
            CurrentHealth -= damage;
            CurrentHealth = Mathf.Max(CurrentHealth, 0);

            float currentHealthRatio = CurrentHealth / MaxHealth;
            OnHealthChanged?.Invoke(currentHealthRatio);
        }

        public bool IsDead => CurrentHealth <= 0;
    }
}