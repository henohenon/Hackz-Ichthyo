using System;
using UnityEngine;

namespace Player.State
{
    public sealed class IdleState : StateBase<IdleStateContext>
    {
        public IdleState(Animator animator, Input input, Action<Type> onChangeState, Transform transform, Rigidbody2D rigidBody, IdleStateContext context) : base(animator, input, onChangeState, transform, rigidBody, context) { }



        public override void OnEnter()
        {
            
        }

        public override void OnUpdate()
        {
            if (Input.Move.action.ReadValue<Vector2>() != Vector2.zero)
            {
                OnChangeState<WalkState>();
            }
        }

        public override void OnExit()
        {
            
        }
    }
}
