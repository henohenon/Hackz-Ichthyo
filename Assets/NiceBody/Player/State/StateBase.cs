using System;
using UnityEngine;

namespace Player
{
    public abstract class StateBase<T> : IState where T : StateContextBase
    {
        private readonly Input input_;
        private readonly Action<Type> onChangeState_;
        private readonly Transform transform_;
        private readonly Rigidbody2D rigidBody_;
        private readonly T context_;

        public StateBase(Input input, Action<Type> onChangeState,  Transform transform, Rigidbody2D rigidBody, T context)
        {
            input_          = input;
            onChangeState_  = onChangeState;
            transform_      = transform;
            rigidBody_      = rigidBody;
            context_        = context;
        }

        public abstract void OnEnter();
        public abstract void OnUpdate();
        public abstract void OnExit();

        /// <summary>
        /// NOTE: 以下サンドボックスパターン
        /// </summary>
        protected T Context             => context_;
        protected Transform Transform   => transform_;
        protected Rigidbody2D Rigidbody => rigidBody_;
        protected Input Input           => input_;
        protected void OnChangeState<StateT>() where StateT : IState => onChangeState_(typeof(StateT));
    }
}