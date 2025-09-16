using UnityEngine;

namespace Player
{
    public sealed class WalkStateContext : StateContextBase
    {
        [SerializeField] private float moveSpeed_;

        public float MoveSpeed_ => moveSpeed_;
    }
}
