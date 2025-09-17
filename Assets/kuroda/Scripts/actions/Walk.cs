using System.Collections;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class Walk : ActionBase
{
    public override async Task DoAction(Context context, float? duration)
    {
        await WalkAsync(context, duration ?? 5f);
    }
    private async UniTask WalkAsync(Context context, float duration)
    {
        float elapsedTime = 0f;
        Rigidbody2D rb = context.EnemyRigidbody2D;
        if (rb == null) { return; }
        while (elapsedTime < duration)
        {
            Vector2 direction = ((Vector2)context.PlayerTransform.position - (Vector2)context.EnemyTransform.position).normalized;
            rb.velocity = direction * context.speed;
            // context.EnemyTransform.position = Vector3.MoveTowards(
            //     context.EnemyTransform.position,
            //     context.PlayerTransform.position,
            //     context.speed * Time.deltaTime
            // );
            elapsedTime += Time.deltaTime;
            await UniTask.Yield();
        }
        rb.velocity = Vector2.zero;
    }
}
