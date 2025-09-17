using System;
using UnityEngine;

namespace Player.State
{
    public sealed class WalkState : StateBase<WalkStateContext>
    {
        public WalkState(Animator animator, Input input, Action<Type> onChangeState, Func<bool> isDeath, Transform transform, Rigidbody2D rigidBody, WalkStateContext context) : base(animator, input, onChangeState, isDeath, transform, rigidBody, context) { }



        public override void OnEnter()
        {
            Animator.Walk();
        }

        public override void OnUpdate()
        {
            Vector2 move = Input.Move.action.ReadValue<Vector2>();
            // Debug.Log(move);

            Rigidbody.velocity = Context.MoveSpeed_ * Time.deltaTime * move;

            if (move.x != 0)
            {
                Transform.rotation = Quaternion.Euler(0, move.x > 0 ? 0 : 180, 0);
            }

            if (move == Vector2.zero)
            {
                OnChangeState<IdleState>();
            }

            if (IsDeath())
            {
                OnChangeState<DeathState>();
            }
        }

        public override void OnExit()
        {
            Animator.WalkOff();
        }
    }
}
