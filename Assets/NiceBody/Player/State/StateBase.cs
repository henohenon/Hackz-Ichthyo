using System;
using UnityEngine;

namespace Player.State
{
    public abstract class StateBase<T> : IState where T : StateContextBase
    {
        private readonly Animator animator_;
        private readonly Input input_;
        private readonly Action<Type> onChangeState_;
        private readonly Func<bool> isDeath_;
        private readonly Transform transform_;
        private readonly Rigidbody2D rigidBody_;
        private readonly T context_;

        public StateBase(Animator animator, Input input, Action<Type> onChangeState, Func<bool> isDeath, Transform transform, Rigidbody2D rigidBody, T context)
        {
            animator_       = animator;
            input_          = input;
            onChangeState_  = onChangeState;
            isDeath_        = isDeath;
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
        protected Animator Animator     => animator_;
        protected Transform Transform   => transform_;
        protected Rigidbody2D Rigidbody => rigidBody_;
        protected Input Input           => input_;
        protected void OnChangeState<StateT>() where StateT : IState => onChangeState_(typeof(StateT));
        protected bool IsDeath() => isDeath_();
    }
}