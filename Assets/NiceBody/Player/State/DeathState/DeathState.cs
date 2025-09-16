using System;
using UnityEngine;

namespace Player.State
{
    [Serializable]
    public sealed class DeathState : StateBase<DeathStateContext>
    {
        public DeathState(Animator animator, Input input, Action<Type> onChangeState, Func<bool> isDeath, Transform transform, Rigidbody2D rigidBody, DeathStateContext context) : base(animator, input, onChangeState, isDeath, transform, rigidBody, context) { }

        public override void OnEnter()
        {
            Animator.Dead();
        }

        public override void OnUpdate()
        {
            
        }

        public override void OnExit()
        {

        }
    }
}
