using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Project.Scripts.Firebase;
using Project.Scripts.Players;
using Project.Scripts.SaveSystem;

namespace Project.Scripts.Enemies
{
    public class EnemyLifecycleManager
    {
        private List<EnemyModel> _enemies = new();
        private readonly EnemyFactory _enemyFactory;
        private readonly EnemySpawnData[] _enemySpawnData;
        private readonly IAnalyticsService _analyticsService;
        private readonly SaveSelection _saveSelection;

        private PlayerModel _player;
        private Action _onAllEnemiesDefeated;
        private int _killCount;

        public EnemyLifecycleManager(
            EnemyFactory enemyFactory,
            EnemySpawnData[] enemySpawnData,
            IAnalyticsService analyticsService,
            SaveSelection saveSelection)
        {
            _enemyFactory = enemyFactory;
            _enemySpawnData = enemySpawnData;
            _analyticsService = analyticsService;
            _saveSelection = saveSelection;
        }

        public void SetPlayer(PlayerModel player)
        {
            _player = player;
        }

        public void SetOnAllEnemiesDefeatedCallback(Action callback)
        {
            _onAllEnemiesDefeated = callback;
        }

        public async UniTask Initialize()
        {
            _enemyFactory.CreateEnemies(_enemySpawnData);

            foreach (var enemy in _enemyFactory.Enemies)
            {
                enemy.SubscribeOnDeath(() => RemoveEnemy(enemy).Forget());
            }

            _enemies = _enemyFactory.Enemies;
        }

        private async UniTaskVoid RemoveEnemy(EnemyModel enemy)
        {
            enemy.UnsubscribeFromDeath();
            enemy.UnsubscribeFromHealthChanged();
            _enemies.Remove(enemy);
            _killCount++;

            _player.PlayerMovement.AddExperience(enemy.EXP);
            await _saveSelection.Save(_player);

            if (_enemies.Count == 0)
            {
                _onAllEnemiesDefeated?.Invoke();
                _analyticsService.LogEnemyDeath(_killCount);
            }
        }
    }
}
