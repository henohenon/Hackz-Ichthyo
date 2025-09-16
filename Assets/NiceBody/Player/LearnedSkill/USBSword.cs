using UnityEngine;
using Cysharp.Threading.Tasks;
using R3.Triggers;
using R3;
using System;

namespace Player.Skill
{
    [CreateAssetMenu(menuName = "Game/Skill/ForwardProjectile")]
    public sealed class USBSword : SkillBase
    {
        [SerializeField] private GameObject projectilePrefab;
        [SerializeField] private float speed = 5f;
        [SerializeField] private float lifetimeSeconds = 10f;
        [SerializeField] private int level2SpreadCount = 3;
        [SerializeField] private float level2SpreadAngle = 30f;
        [SerializeField] private int level3ChainCount = 3;
        [SerializeField] private float level3ChainInterval = 0.5f;

        public override void OnActionLevel1(UseSkillContext context)
        {
            var player = context.Player;
            if (player == null || projectilePrefab == null) return;

            Vector3 spawnPosition = player.transform.position + player.transform.right * 1f;
            Quaternion rotation = Quaternion.LookRotation(Vector3.forward, player.transform.up);

            GameObject projectile = Instantiate(projectilePrefab, spawnPosition, rotation);
            MoveProjectileReactive(projectile, player.transform.right, speed, lifetimeSeconds).Forget();
        }

        public override void OnActionLevel2(UseSkillContext context)
        {
            var player = context.Player;
            if (player == null || projectilePrefab == null) return;

            Vector3 origin = player.transform.position;
            Vector3 forward = player.transform.right;

            for (int i = 0; i < level2SpreadCount; i++)
            {
                float angleOffset = level2SpreadAngle * (i - (level2SpreadCount - 1) / 2f);
                Vector3 dir = Quaternion.Euler(0, 0, angleOffset) * forward;
                GameObject projectile = Instantiate(projectilePrefab, origin + dir * 1f, Quaternion.identity);
                MoveProjectileReactive(projectile, dir.normalized, speed, lifetimeSeconds).Forget();
            }
        }

        public override void OnActionLevel3(UseSkillContext context)
        {
            var player = context.Player;
            if (player == null || projectilePrefab == null) return;

            Vector3 spawnPosition = player.transform.position + player.transform.right * 1f;
            Vector3 direction = player.transform.right;

            GameObject rootProjectile = Instantiate(projectilePrefab, spawnPosition, Quaternion.identity);
            MoveProjectileReactive(rootProjectile, direction, speed, lifetimeSeconds).Forget();

            ChainSpawn(rootProjectile.transform.position, direction).Forget();
        }

        private async UniTaskVoid ChainSpawn(Vector3 origin, Vector3 direction)
        {
            for (int i = 0; i < level3ChainCount; i++)
            {
                await UniTask.Delay(TimeSpan.FromSeconds(level3ChainInterval));
                GameObject chained = Instantiate(projectilePrefab, origin + direction * (i + 1), Quaternion.identity);
                MoveProjectileReactive(chained, direction, speed, lifetimeSeconds).Forget();
            }
        }

        private async UniTaskVoid MoveProjectileReactive(GameObject projectile, Vector3 direction, float speed, float lifetime)
        {
            if (!projectile.TryGetComponent<Rigidbody2D>(out var rb)) return;

            var disposable = projectile
                .UpdateAsObservable()
                .Subscribe(_ =>
                {
                    rb.MovePosition(rb.position + (Vector2)(direction * speed * Time.deltaTime));
                });

            await UniTask.Delay(TimeSpan.FromSeconds(lifetime));

            disposable.Dispose();
            if (projectile != null) UnityEngine.Object.Destroy(projectile);
        }
    }
}