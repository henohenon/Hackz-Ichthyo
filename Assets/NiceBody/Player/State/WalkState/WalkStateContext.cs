using UnityEngine;

namespace Player.State
{
    public sealed class WalkStateContext : StateContextBase
    {
        [SerializeField] private float moveSpeed_;

        public float MoveSpeed => moveSpeed_;

        public void SetSpeed(float speed)
        {
            moveSpeed_ = speed;
        }
    }
}
