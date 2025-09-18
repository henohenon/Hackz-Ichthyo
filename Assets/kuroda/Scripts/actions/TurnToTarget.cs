using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using LitMotion;
using LitMotion.Extensions;
using Unity.VisualScripting;
using UnityEngine;

namespace kuroda.Scripts.actions
{
    public class TurnToTarget: ActionBase
    {
        private MotionHandle _handle;
        
        public override async Task DoAction(Context context, float? duration)
        {
            await TurnAsync(context, duration ?? 0.3f);
        }
        private async UniTask TurnAsync(Context context, float duration)
        {
            Rigidbody2D rb = context.EnemyRigidbody2D;
            var current = rb.transform.eulerAngles.z;
            var direction = context.PlayerTransform.position - context.EnemyTransform.position;
            var angleRad = Mathf.Atan2(direction.y, direction.x); // ラジアンを取得
            var angleDeg = angleRad * Mathf.Rad2Deg; // 度数に変換

            var handle = LMotion.Create(context.EnemyTransform.eulerAngles.z, angleDeg, duration)
                    .Bind(x => rb.transform.eulerAngles = new Vector3(0, 0, x));
            await handle;
        }

        public override void Dispose()
        {
            base.Dispose();
            _handle.TryCancel();
        }
    }
}