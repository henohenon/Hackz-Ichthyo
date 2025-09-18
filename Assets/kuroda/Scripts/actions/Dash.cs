using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace kuroda.Scripts.actions
{
    public class Dash: ActionBase
    {
        
        public override async Task DoAction(Context context, float? duration)
        {
            await DashAsync(context, duration ?? 1f);
        }
        private async UniTask DashAsync(Context context, float duration)
        {
            float elapsedTime = 0f;
            Rigidbody2D rb = context.EnemyRigidbody2D;
            if (rb == null) { return; }
            while (elapsedTime < duration)
            {
                rb.velocity = context.EnemyTransform.right * context.speed;
                elapsedTime += Time.deltaTime;
                await UniTask.Yield();
            }
            rb.velocity = Vector2.zero;
        }
    }
}