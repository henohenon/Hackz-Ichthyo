using System;
using Cysharp.Threading.Tasks;
using LitMotion;
using LitMotion.Extensions;
using NiceBody.Player.LearnedSkill;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Player
{
    public class DrawingDamage: MonoBehaviour
    {
        [SerializeField]
        private DrawingBomb _drawingBomb;
        private float _damage;
        private float _attractionForce;
        private float _speed;
        public void Initialize(float size, float damage, float speed, float lifeTime, float attractionForce, int bombCount)
        {
            _damage = damage;
            _attractionForce = attractionForce;
            _speed = speed;
            transform.localScale = Vector3.one * size;

            OnDestroyAsync(lifeTime, bombCount).Forget();
            
            Rotate();
        }

        private MotionHandle _handle;
        private void Rotate()
        {
            var target = transform.rotation;
            target *= Quaternion.Euler(0, 0, Random.Range(-100, 100));
            _handle.TryComplete();
            _handle = LMotion.Create(transform.rotation, target, Random.Range(0.05f, 1.2f)).WithOnComplete(() => Rotate()).BindToRotation(transform);
            
        }
    
        private async UniTaskVoid OnDestroyAsync(float secs, int bombCount)
        {
            await UniTask.Delay(TimeSpan.FromSeconds(secs));
            var rotate = transform.rotation.z;
            for (var i = 0; i < bombCount; i++)
            {
                rotate += Random.Range(0, 90);
                var rot = Quaternion.Euler(0, 0, rotate);
                var instance = Instantiate(_drawingBomb, transform.position, rot);
                instance.Initialize(transform.localScale.x, 3, 0.5f, _damage, _attractionForce);
            }
            _handle.TryCancel();
            Destroy(gameObject);
        }

        private void Update()
        {
            var forward = Vector3.right * (_speed * Time.deltaTime);
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