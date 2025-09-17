using System.Threading.Tasks;
using UnityEngine;
public class Teleport : ActionBase
{
    // プレイヤーからどれだけ前に出現するかの距離
    public float offsetDistance = 3f;
    public override async Task DoAction(Context context, float? duration)
    {        // プレイヤーの位置を取得
        Vector3 playerPos = context.PlayerTransform.position;

        // プレイヤーの向いている方向（前方ベクトル）を取得
        Vector2 playerDirection2 = context.PlayerInput.Move.action.ReadValue<Vector2>();
        Vector3 playerDirection3 = new Vector3(playerDirection2.x, playerDirection2.y, 0);
        
        // プレイヤーの位置から、向いている方向にoffsetDistanceだけ進んだ位置を計算
        Vector3 teleportPosition = playerPos + playerDirection3 * offsetDistance;
        teleportPosition.z = -1f;
        
        // 計算した位置に敵をテレポートさせる
        context.EnemyTransform.position = teleportPosition;

        return;
    }
}
