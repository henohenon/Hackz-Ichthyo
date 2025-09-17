using System.Threading.Tasks;
using UnityEngine;
using Cysharp.Threading.Tasks;
public class USBSord : ActionBase
{
    // === 突進のパラメータ（Inspectorから調整できるようにpublicにしても良い） ===
    private float rushSpeedMultiplier = 20.0f; // ②突進時の速度倍率（通常のspeedの何倍か）
    private float cooldownTime = 1.0f;   // ③突進後の硬直時間（秒）
    // =======================================================================

    public override async Task DoAction(Context context, float? duration)
    {
        if (duration == null)
        {
            duration = 2f;
        }
        Rigidbody2D rb = context.EnemyRigidbody2D;
        if (rb == null) { return; }

        // --- ② 突進フェーズ ---
        Vector2 rushDirection = ((Vector2)context.PlayerTransform.position - (Vector2)context.EnemyTransform.position).normalized;
        rb.velocity = rushDirection * context.speed * rushSpeedMultiplier;
        // context.EnemySpriteRenderer.color = Color.white; // 色を戻す
        await UniTask.Delay((int)(duration * 1000)); // 指定時間、突進を継続

        // --- ③ 硬直フェーズ ---
        rb.velocity = Vector2.zero;
        await UniTask.Delay((int)(cooldownTime * 1000)); // 指定時間、硬直
    }
}
