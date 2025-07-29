using Project.Scripts.Animations.Character;
using UnityEngine;

namespace Project.Scripts.Players
{
    public class PlayerView : MonoBehaviour
    {
        private ICharacterAnimator _animator;
        private PlayerModel _playerModel;

        public void Initialize(PlayerModel playerModel, ICharacterAnimator animator)
        {
            _playerModel = playerModel;
            _animator = animator;
        }

        public void SubscribeToModel()
        {
            _playerModel.OnDeath += Die;
            _playerModel.OnAttack += _animator.PlayAttack;
            _playerModel.OnWalk += _animator.PlayWalk;
            _playerModel.OnStopWalk += _animator.StopWalk;
        }

        private void Die()
        {
            _playerModel.OnAttack -= _animator.PlayAttack;
            _playerModel.OnWalk -= _animator.PlayWalk;
            _playerModel.OnStopWalk -= _animator.StopWalk;
            Destroy(gameObject);
        }
    }
}