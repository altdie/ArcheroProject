using UnityEngine;

namespace Project.Scripts.Animations.Character
{
    public class CharacterAnimator : ICharacterAnimator
    {
        private readonly Animator _animator;

        public CharacterAnimator(Animator animator)
        {
            this._animator = animator;
        }

        public void PlayWalk()
        {
            _animator.SetBool("isWalking", true);
        }

        public void StopWalk()
        {
            _animator.SetBool("isWalking", false);
        }

        public void PlayAttack()
        {
            _animator.SetTrigger("Attack");
        }
    }
}