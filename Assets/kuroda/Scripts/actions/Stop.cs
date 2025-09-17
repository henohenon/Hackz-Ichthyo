using System.Threading.Tasks;
using UnityEngine;
using Cysharp.Threading.Tasks;
using System;
public class Stop : ActionBase
{
    public float flashInterval = 0.15f; // 点滅の間隔（秒）
    public override async Task DoAction(Context context, float? duration)
    {
        await Flash(context, duration ?? 1f);
    }
    private async UniTask Flash(Context context, float duration)
    {
        float elapsedTime = 0f;
        while (elapsedTime < duration) // 常にループして点滅を続ける
        {
            // 点滅の間、消える
            context.EnemySpriteRenderer.enabled = false;
            await UniTask.Delay(TimeSpan.FromSeconds(flashInterval));
            elapsedTime += flashInterval;

            if (elapsedTime >= duration) break;

            // 点滅の間、表示する
            context.EnemySpriteRenderer.enabled = true;
            await UniTask.Delay(TimeSpan.FromSeconds(flashInterval));
            elapsedTime += Time.deltaTime;
        }
        context.EnemySpriteRenderer.enabled = true;
    }
}
