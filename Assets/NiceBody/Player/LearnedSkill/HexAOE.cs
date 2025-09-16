using UnityEngine;
using Cysharp.Threading.Tasks;
using System;

public class HexAOE : MonoBehaviour
{
    [Header("演出パラメータ")]
    [SerializeField] private float duration = 2f;
    [SerializeField] private float rotationSpeed = 180f;
    [SerializeField] private AnimationCurve shrinkCurve = AnimationCurve.EaseInOut(0, 1, 1, 0);

    [Header("ダメージ設定")]
    [SerializeField] private LayerMask damageLayer;
    [SerializeField] private float attractionForce = 2f;

    [SerializeField] private float size_;
    [SerializeField] private float damage_;
    [SerializeField] private bool attract_;

    public void Initialize(float size, float damage)
    {
        size_ = size;
        damage_ = damage;
        transform.localScale = Vector3.one * size_;
        _ = RotateAndShrinkAsync(); // fire-and-forget
    }

    public void EnableAttraction()
    {
        attract_ = true;
    }

    private async UniTaskVoid RotateAndShrinkAsync()
    {
        float time = 0f;
        Vector3 initialScale = Vector3.one * size_;

        while (time < duration)
        {
            transform.Rotate(Vector3.forward, rotationSpeed * Time.deltaTime);

            float t = time / duration;
            float scaleFactor = shrinkCurve.Evaluate(t);
            transform.localScale = initialScale * scaleFactor;

            time += Time.deltaTime;
            await UniTask.Yield();
        }

        Destroy(gameObject);
    }

    private void Update()
    {
        if (!attract_) return;

        var colliders = Physics2D.OverlapCircleAll(transform.position, size_, damageLayer);
        foreach (var col in colliders)
        {
            var rb = col.attachedRigidbody;
            if (rb != null)
            {
                Vector2 dir = (transform.position - rb.transform.position).normalized;
                rb.velocity += dir * attractionForce * Time.deltaTime;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (((1 << other.gameObject.layer) & damageLayer) == 0) return;

        var target = other.GetComponent<EnemyBase>();
        if (target != null)
        {
            //target.OnDamage((int)damage_);
            Debug.LogError("jfiejiwoajefioa");
        }
    }
}