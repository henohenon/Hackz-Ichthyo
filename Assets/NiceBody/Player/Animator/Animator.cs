using UnityEngine;
using System;

namespace Player
{
    [Serializable]
    public sealed class Animator
    {
        [SerializeField] private UnityEngine.Animator animator_;

        public void Jump()
        {
            animator_.SetBool("Jump", true);
        }

        public void JumpOff()
        {
            animator_.SetBool("Jump", false);
        }

        public void Dead()
        {
            animator_.SetBool("Dead", true);
        }

        public void DeadOff()
        {
            animator_.SetBool("Dead", false);
        }
        public void Walk()
        {
            animator_.SetBool("Walk", true);
        }

        public void WalkOff()
        {
            animator_.SetBool("Walk", false);
        }
        public void Run()
        {
            animator_.SetBool("Run", true);
        }
        public void RunOff()
        {
            animator_.SetBool("Run", false);
        }
        public void Attack()
        {
            animator_.SetBool("Attack", true);
        }
        public void AttackOff()
        {
            animator_.SetBool("Attack", false);
        }
    }
}

