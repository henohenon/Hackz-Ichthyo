using System;
using UnityEngine;

namespace Player.State
{
    public sealed class WalkState : StateBase<WalkStateContext>
    {
        public WalkState(Animator animator, Input input, Action<Type> onChangeState, Transform transform, Rigidbody2D rigidBody, WalkStateContext context) : base(animator, input, onChangeState, transform, rigidBody, context) { }



        public override void OnEnter()
        {
            Animator.Walk();
        }

        public override void OnUpdate()
        {
            Debug.Log(Input.Move.action.ReadValue<Vector2>());
            Rigidbody.velocity = Context.MoveSpeed_ * Time.deltaTime * Input.Move.action.ReadValue<Vector2>();

            if(Input.Move.action.ReadValue<Vector2>() == Vector2.zero)
            {
                OnChangeState<IdleState>();
            }
        }

        public override void OnExit()
        {
            Animator.WalkOff();
        }
    }
}
