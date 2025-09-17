using UnityEngine;
using Cysharp.Threading.Tasks;
using System;
using LitMotion.Animation;
using UnityEngine.Serialization;

[RequireComponent(typeof(LitMotionAnimation))]
public class HexAOE : MonoBehaviour
{
    [SerializeField] private float lifeTime = 0.3f;
    
    private float _attractionForce = 2f;
    private float _damage;

    public void Initialize(float size, float damage, float attractionForce)
    {
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
            rb.AddForce(dir * _attractionForce, ForceMode2D.Impulse);
        }
    }
}