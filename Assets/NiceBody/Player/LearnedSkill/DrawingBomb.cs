using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Random = UnityEngine.Random;

namespace NiceBody.Player.LearnedSkill
{
    public class DrawingBomb: MonoBehaviour
    {
        private float _speed;
        private float _damage;
        private float _attractionForce;
        
        public void Initialize(float size, float speed, float lifeTime, float damage, float attractionForce)
        {
            _speed = speed;
            _damage = damage;
            _attractionForce = attractionForce;
            transform.localScale = Vector3.one * size;
            OnDestroyAsync(lifeTime).Forget();
        }
    
        private async UniTaskVoid OnDestroyAsync(float secs)
        {
            await UniTask.Delay(TimeSpan.FromSeconds(secs));
            Destroy(gameObject);
        }
        
        private void Update()
        {
            var forward = transform.right * (_speed * Time.deltaTime);
            transform.Translate(forward);
        }
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.TryGetComponent<EnemyBase>(out var target))
            {
                target.OnDamage(_damage);
            }
            var rb = other.attachedRigidbody;
            if (rb != null)
            {
                Vector2 dir = (transform.position - rb.transform.position).normalized;
                rb.AddForce(-dir * _attractionForce, ForceMode2D.Impulse);
            }
        }
    }
}