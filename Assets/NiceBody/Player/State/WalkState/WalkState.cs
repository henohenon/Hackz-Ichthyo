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
            Vector2 move = Input.Move.action.ReadValue<Vector2>();
            Debug.Log(move);

            Rigidbody.velocity = Context.MoveSpeed_ * Time.deltaTime * move;

            // 向きの反転（rotation.y を 0 または 180 に）
            if (move.x != 0)
            {
                Transform.rotation = Quaternion.Euler(0, move.x > 0 ? 0 : 180, 0);
            }

            if (move == Vector2.zero)
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
