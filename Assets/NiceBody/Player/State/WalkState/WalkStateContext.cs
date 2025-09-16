using UnityEngine;

namespace Player.State
{
    public sealed class WalkStateContext : StateContextBase
    {
        [SerializeField] private float moveSpeed_;

        public float MoveSpeed_ => moveSpeed_;
    }
}
