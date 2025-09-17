using System;
using UnityEngine;

namespace Player.State
{
    public sealed class IdleState : StateBase<IdleStateContext>
    {
        public IdleState(Animator animator, Input input, Action<Type> onChangeState, Func<bool> isDeath, Transform transform, Rigidbody2D rigidBody, IdleStateContext context) : base(animator, input, onChangeState, isDeath, transform, rigidBody, context) { }



        public override void OnEnter()
        {
            
        }

        public override void OnUpdate()
        {
            Rigidbody.velocity = Vector2.zero;

            if (Input.Move.action.ReadValue<Vector2>() != Vector2.zero)
            {
                OnChangeState<WalkState>();
            }

            if(IsDeath())
            {
                OnChangeState<DeathState>();
            }
        }

        public override void OnExit()
        {
            
        }
    }
}
