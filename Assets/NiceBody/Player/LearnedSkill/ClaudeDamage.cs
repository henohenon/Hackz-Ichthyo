using UnityEngine;

namespace Player.Skill
{
    public class ClaudeDamage : MonoBehaviour
    {
        private float damage_;

        public void Initialize(float damage)
        {
            damage_ = damage;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.TryGetComponent(out EnemyBase target))
            {
                //target.OnDamage((int)damage_);
                UnityEngine.Debug.LogError("jifejajfioe");
            }
        }
    }
}