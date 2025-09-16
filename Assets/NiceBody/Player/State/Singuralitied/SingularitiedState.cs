using System;
using UnityEngine;

namespace Player
{
    public sealed class SingularitiedState : StateBase<SingularitiedStateContext>
    {
        public SingularitiedState(Input input, Action<Type> onChangeState, Transform transform, Rigidbody2D rigidBody, SingularitiedStateContext context) : base(input, onChangeState, transform, rigidBody, context) { }


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
