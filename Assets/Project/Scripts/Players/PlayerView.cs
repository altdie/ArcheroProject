using System;
using System.Collections;
using System.Collections.Generic;
using Project.Scripts.Enemies;
using Project.Scripts.HealthInfo;
using Project.Scripts.PlayerModels;
using UnityEngine;

namespace Project.Scripts.Players
{
    public class PlayerView : MonoBehaviour
    {
        public event Action OnEntityDeath;
        private PlayerModel _playerModel;

        public void Initialize(PlayerModel playerModel)
        {
            _playerModel = playerModel;
            _playerModel.OnDeath += Die;
        }

        private void Die()
        {
            OnEntityDeath?.Invoke();
            Destroy(gameObject);
        }
    }
}