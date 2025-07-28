using UnityEngine;

namespace Project.Scripts.Animations.Enemy
{
    public class EnemyAnimator : IEnemyAnimator
    {
        private readonly Animator _animator;

        public EnemyAnimator(Animator animator)
        {
            this._animator = animator;
        }

        public void PlayGetHit()
        {
            _animator.SetTrigger("GetHit");
        }

        public void PlayDie()
        {
            _animator.SetTrigger("Die");
        }

        public void PlayAttack()
        {
            _animator.SetTrigger("Attack");
        }
    }
}
