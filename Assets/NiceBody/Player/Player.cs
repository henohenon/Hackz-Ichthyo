using R3;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    [RequireComponent(typeof(Rigidbody2D))]
    public sealed class Player : MonoBehaviour
    {
        [SerializeField] Input input_;

        [SerializeField] private Health health_;
        [SerializeField] private IQ singularityIq_;
        [SerializeField] private SerializableReactiveProperty<IQ> iq_;
        [SerializeField] SuperComputer superComputer_;

        [SerializeField] List<StateContextBase> stateContextBase_;
        private readonly Dictionary<Type, IState> states_ = new();
        private IState state_;

        public Health Health_ => health_;
        public ReadOnlyReactiveProperty<IQ> IQ => iq_;

        private void Awake()
        {
            input_.Init();
            RegisterState<IdleState, IdleStateContext>();
            RegisterState<WalkState, WalkStateContext>();
            RegisterState<SingularitiedState, SingularitiedStateContext>();
            OnChangeState(typeof(IdleState));
        }

        private void Update()
        {
            state_?.OnUpdate();
            superComputer_.Tick(iq_);

            if (IQ.CurrentValue > singularityIq_)
            {
                OnChangeState(typeof(SingularitiedState));
            }
        }

        public void OnChangeState(Type newType)
        {
            if (!typeof(IState).IsAssignableFrom(newType))
            {
                Debug.LogError($"Type {newType.Name} does not implement IState.");
                return;
            }

            Type previousType = state_?.GetType();
            Debug.Log($"Changing state from {previousType?.Name ?? "None"} to {newType.Name}");

            if (states_.TryGetValue(newType, out var newState))
            {
                state_?.OnExit();
                state_ = newState;
                state_.OnEnter();
            }
            else
            {
                Debug.LogWarning($"State of type {newType.Name} is not registered.");
            }
        }

        private void RegisterState<TState, TContext>() where TState : IState where TContext : StateContextBase
        {
            foreach (var context in stateContextBase_)
            {
                if (context is TContext matchedContext)
                {
                    var state = (IState)Activator.CreateInstance(
                                            typeof(TState),
                                            input_,
                                            new Action<Type>(OnChangeState), // ← 明示的にキャスト
                                            transform,
                                            GetComponent<Rigidbody2D>(),
                                            matchedContext
                                        );

                    states_.Add(typeof(TState), state);
                    return;
                }
            }

            Debug.LogWarning($"Context of type {typeof(TContext).Name} not found.");
        }
    }
}
