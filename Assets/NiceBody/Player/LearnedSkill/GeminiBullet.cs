using UnityEngine;
public sealed class GeminiBullet : MonoBehaviour
{
    private float speed_;
    private int damage_;
    private float lifetime_ = 5f;
    private int pierceCount_;

    public void Initialize(float speed, int damage, float lifeTime, int pierceCount)
    {
        speed_ = speed;
        damage_ = damage;
        lifetime_ = lifeTime;
        pierceCount_ = pierceCount;
        Destroy(gameObject, lifetime_);
    }
    private void Update()
    {
        var forward = Vector3.right * (speed_ * Time.deltaTime);
        transform.Translate(forward);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        EnemyBase enemy = other.GetComponent<EnemyBase>();
        if (enemy == null)
        {
            return;
        }
        enemy.OnDamage(damage_);
        pierceCount_--;
        if (pierceCount_ < 0)
        {
            Destroy(gameObject);
        }
    }
}