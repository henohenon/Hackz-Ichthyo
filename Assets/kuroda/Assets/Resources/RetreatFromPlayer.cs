using Cysharp.Threading.Tasks;
using System.Threading.Tasks;
using UnityEngine;

namespace kuroda.Scripts.actions
{
    public class RetreatFromPlayer : ActionBase
    {
        private readonly float speed = 0.05f;
        private readonly float minDistance = 0;
        private readonly float maxDistance = 3.0f;


        public override async Task DoAction(Context context, float? duration = 0)
        {
            float distance = Vector3.Distance(context.EnemyTransform.position, context.PlayerTransform.position);

            // 距離が maxDistance を超えていない場合のみ逃走
            if (distance < maxDistance)
            {
                Vector3 direction = (context.EnemyTransform.position - context.PlayerTransform.position).normalized;
                context.EnemyTransform.position += direction * speed;
            }

            await UniTask.WaitForSeconds((float)duration);
        }
    }
}