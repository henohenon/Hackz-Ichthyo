using System.Threading.Tasks;
using UnityEngine;
public class Teleport : ActionBase
{
    // プレイヤーからどれだけ前に出現するかの距離
    public float offsetDistance = 2.0f;
    public override async Task DoAction(Context context, float? duration)
    {        // プレイヤーの位置を取得
        Vector3 playerPos = context.PlayerTransform.position;
        
        // プレイヤーの向いている方向（前方ベクトル）を取得
        Vector3 playerForward = context.PlayerTransform.forward;
        
        // プレイヤーの位置から、向いている方向にoffsetDistanceだけ進んだ位置を計算
        Vector3 teleportPosition = playerPos + playerForward * offsetDistance;
        
        // 計算した位置に敵をテレポートさせる
        context.EnemyTransform.position = teleportPosition;

        return;
    }
}
