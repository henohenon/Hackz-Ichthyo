using UnityEngine;
using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;

namespace Player.Skill
{
    [CreateAssetMenu(menuName = "Game/Skill/Claude")]
    public sealed class Claude : SkillBase
    {
        [Header("軌跡設定")]
        [SerializeField] private GameObject linePrefab; // LineRenderer を含むプレハブ
        [SerializeField] private float squiggleAmplitude = 0.2f;
        [SerializeField] private float squiggleFrequency = 10f;
        [SerializeField] private float drawInterval = 0.05f;

        [Header("基本パラメータ")]
        [SerializeField] private float baseSpeed = 3f;
        [SerializeField] private float baseDuration = 5f;
        [SerializeField] private float baseDamage = 10f;

        [Header("爆発演出")]
        [SerializeField] private GameObject burstEffectPrefab;
        [SerializeField] private float burstDamage = 30f;
        [SerializeField] private int burstRayCount = 5;
        [SerializeField] private float burstForce = 5f;

        public override void OnActionLevel1(UseSkillContext context)
        {
            DrawSquigglyLine(context.Player.transform.position, context.Player.transform.right, baseSpeed, baseDuration, baseDamage).Forget();
        }

        public override void OnActionLevel2(UseSkillContext context)
        {
            float speed = baseSpeed * 1.5f;
            float damage = baseDamage * 1.5f;
            DrawSquigglyLine(context.Player.transform.position, context.Player.transform.right, speed, baseDuration, damage).Forget();
        }

        public override void OnActionLevel3(UseSkillContext context)
        {
            float speed = baseSpeed * 2f;
            float damage = baseDamage * 2f;
            DrawSquigglyLine(context.Player.transform.position, context.Player.transform.right, speed, baseDuration, damage, burst: true).Forget();
        }

        private async UniTaskVoid DrawSquigglyLine(Vector3 origin, Vector3 direction, float speed, float duration, float damage, bool burst = false)
        {
            var lineObj = GameObject.Instantiate(linePrefab, origin, Quaternion.identity);
            var line = lineObj.GetComponent<LineRenderer>();
            var points = new List<Vector3>();

            float time = 0f;
            Vector3 current = origin;

            while (time < duration)
            {
                float offset = Mathf.Sin(time * squiggleFrequency) * squiggleAmplitude;
                Vector3 squiggled = current + Vector3.up * offset;

                points.Add(squiggled);
                line.positionCount = points.Count;
                line.SetPositions(points.ToArray());

                CreateDamagePoint(squiggled, damage);

                current += direction * speed * drawInterval;
                time += drawInterval;
                await UniTask.Delay(TimeSpan.FromSeconds(drawInterval));
            }

            if (burst)
            {
                ExplodeBurst(current);
            }

            GameObject.Destroy(lineObj, 0.5f);
        }

        private void CreateDamagePoint(Vector3 position, float damage)
        {
            var hitbox = new GameObject("ClaudeHitPoint");
            hitbox.transform.position = position;
            var area = hitbox.AddComponent<CircleCollider2D>();
            area.radius = 0.2f;
            var dmg = hitbox.AddComponent<ClaudeDamage>();
            dmg.Initialize(damage);
            GameObject.Destroy(hitbox, 0.2f);
        }

        private void ExplodeBurst(Vector3 center)
        {
            if (burstEffectPrefab != null)
            {
                GameObject.Instantiate(burstEffectPrefab, center, Quaternion.identity);
            }

            for (int i = 0; i < burstRayCount; i++)
            {
                float angle = (360f / burstRayCount) * i;
                Vector3 dir = Quaternion.Euler(0, 0, angle) * Vector3.right;
                var ray = new GameObject("ClaudeBurstRay");
                ray.transform.position = center;
                var rb = ray.AddComponent<Rigidbody2D>();
                rb.AddForce(dir * burstForce, ForceMode2D.Impulse);
                var dmg = ray.AddComponent<ClaudeDamage>();
                dmg.Initialize(burstDamage);
                GameObject.Destroy(ray, 0.5f);
            }
        }
    }
}