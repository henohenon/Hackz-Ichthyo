using UnityEngine;
using Cysharp.Threading.Tasks;
using R3.Triggers;
using R3;

namespace Player.Skill
{
    [CreateAssetMenu(menuName = "Game/Skill/ForwardProjectile")]
    public sealed class USBSword : SkillBase
    {
        [SerializeField] private GameObject projectilePrefab;
        [SerializeField] private float speed = 5f;
        [SerializeField] private float lifetimeSeconds = 10f;

        public override void OnAction(UseSkillContext context)
        {
            var player = context.Player;
            if (player == null || projectilePrefab == null)
                return;

            Vector3 spawnPosition = player.transform.position + player.transform.right * 1f;
            Quaternion rotation = Quaternion.LookRotation(Vector3.forward, player.transform.up);

            GameObject projectile = Instantiate(projectilePrefab, spawnPosition, rotation);

            MoveProjectileReactive(projectile, player.transform.right, speed, lifetimeSeconds).Forget();
        }

        private async UniTaskVoid MoveProjectileReactive(GameObject projectile, Vector3 direction, float speed, float lifetime)
        {
            if (!projectile.TryGetComponent<Rigidbody2D>(out var rb))
                return;

            var disposable = projectile
                .UpdateAsObservable()
                .Subscribe(_ =>
                {
                    rb.MovePosition(rb.position + (Vector2)(direction * speed * Time.deltaTime));
                });

            await UniTask.Delay(System.TimeSpan.FromSeconds(lifetime));

            disposable.Dispose();
            if (projectile != null) Object.Destroy(projectile);
        }
    }
}