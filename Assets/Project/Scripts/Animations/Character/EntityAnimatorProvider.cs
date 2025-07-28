using UnityEngine;

namespace Project.Scripts.Animations.Character
{
    public class EntityAnimatorProvider : MonoBehaviour
    {
        [SerializeField] private Animator animator;

        public Animator Animator => animator;
    }
}