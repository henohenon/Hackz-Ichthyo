using Alchemy.Inspector;
using Player.Skill;
using Player.State;
using R3;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using NiceBody.Player.LearnedSkill;
using GameUnity;


namespace Player
{
    [RequireComponent(typeof(Rigidbody2D))]
    public sealed class Player : MonoBehaviour
    {
        [SerializeField] private Input input_;
        [SerializeField] private SuperComputer superComputer_;
        [SerializeField] private Animator animator_;
        [SerializeField] private LearnedSkillGroup learnedSkillGroup_;

        [SerializeField] private Health maxHealth_;
        [SerializeField] private SerializableReactiveProperty<Health> health_ = new(new Health(10));
        [SerializeField] private IQ singularityIq_;
        [SerializeField] private SerializableReactiveProperty<IQ> iq_ = new();

        [SerializeField] private List<StateContextBase> stateContextBase_;
        [SerializeField] private List<SelectLernSkillGroup> skills;
        private readonly Dictionary<Type, IState> states_ = new();
        private IState state_;

        public Health MaxHealth => maxHealth_;
        public ReadOnlyReactiveProperty<Health> Health => health_;
        public ReadOnlyReactiveProperty<IQ> IQ => iq_;
        public IQ SingularityIq_ => singularityIq_;
        public LearnedSkillGroup LearnedSkillGroup_ => learnedSkillGroup_;
        public SuperComputer SuperComputer_ => superComputer_;

        private void Awake()
        {
            input_.Init();
            learnedSkillGroup_.Init(this);
            RegisterState<IdleState, IdleStateContext>();
            RegisterState<WalkState, WalkStateContext>();
            RegisterState<SingularitiedState, SingularitiedStateContext>();
            RegisterState<DeathState, DeathStateContext>();
            OnChangeState(typeof(IdleState));
        }

        private void Update()
        {
            state_?.OnUpdate();
            SuperComputer_.Tick(iq_);

            if (IQ.CurrentValue > SingularityIq_)
            {
                OnChangeState(typeof(SingularitiedState));
            }
        }

        public Input GetInput() => input_;

        public Vector2 GetMoveInput() => input_.Move.action.ReadValue<Vector2>();

        public void OnChangeState(Type newType)
        {
            if (!typeof(IState).IsAssignableFrom(newType))
            {
                Debug.LogError($"Type {newType.Name} does not implement IState.");
                return;
            }

            Type previousType = state_?.GetType();
            // Debug.Log($"Changing state from {previousType?.Name ?? "None"} to {newType.Name}");

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
            var matchedContext = stateContextBase_.OfType<TContext>().FirstOrDefault();
            if (matchedContext == null)
            {
                Debug.LogWarning($"Context of type {typeof(TContext).Name} not found.");
                return;
            }

            var state = (IState)Activator.CreateInstance(
                            typeof(TState),
                            animator_,
                            input_,
                            new Action<Type>(OnChangeState),
                            new Func<bool>(IsDeath),
                            transform,
                            GetComponent<Rigidbody2D>(),
                            matchedContext
                        );

            states_.Add(typeof(TState), state);

        }

        public T GetState<T>() where T : class, IState
        {
            if (states_.TryGetValue(typeof(T), out var state))
            {
                return state as T;
            }

            Debug.LogWarning($"State of type {typeof(T).Name} is not registered.");
            return null;
        }

        [Button]
        public void OnDamage(int damage)
        {
<<<<<<< Updated upstream
            health_.OnNext(new Health(health_.Value.Value - damage));
            Debug.Log("PlayerのHP: " + health_.Value.Value);
=======
            Sound.PlaySE(SoundEffectType.Damage);
            health_.OnNext(health_.Value - damage);
            Debug.Log("PlayerのHP: " + health_.Value);
>>>>>>> Stashed changes
        }

        private bool IsDeath() => health_.Value.Value <= 0;

        private int killCount = 0;
        [SerializeField]
        private int[] skillUpKillCounts = {
            5, 12, 22, 30, 43, 55, 60, 100
        };
        public void AddIQ(IQ iq)
        {
            killCount++;
            if (Array.IndexOf(skillUpKillCounts, killCount) != -1)
            {
                learnedSkillGroup_.SelectLearnSkill();
            }

            iq_.OnNext(iq_.Value + iq);
        }

        private int getCounter()
        {
            Enemy1 enemy1 = new Enemy1();
            Enemy2 enemy2 = new Enemy2();
            return enemy1.getNumber() + enemy2.getNumber();
        }
    }
}