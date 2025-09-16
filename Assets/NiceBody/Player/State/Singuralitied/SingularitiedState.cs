using System;
using UnityEngine;

namespace Player.State
{
    public sealed class SingularitiedState : StateBase<SingularitiedStateContext>
    {
        public SingularitiedState(Animator animator, Input input, Action<Type> onChangeState, Transform transform, Rigidbody2D rigidBody, SingularitiedStateContext context) : base(animator, input, onChangeState, transform, rigidBody, context) { }


        public override void OnEnter()
        {
            
        }

        public override void OnExit()
        {
            
        }

        public override void OnUpdate()
        {
            
        }
    }
}
