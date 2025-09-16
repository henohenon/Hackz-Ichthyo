    using System.Collections;
    using System.Threading.Tasks;
    using Cysharp.Threading.Tasks;
    using UnityEngine;

    public class Walk : ActionBase
    {
        public override async Task DoAction(Context context, float? duration)
        {
            await WalkAsync(context, duration ?? 5f);
            Debug.Log("5秒経ったよ");
        }
        private async UniTask WalkAsync(Context context, float duration)
        {
            float elapsedTime = 0f;
            while (elapsedTime < duration)
            {
                context.EnemyTransform.position = Vector3.MoveTowards(
                    context.EnemyTransform.position,
                    context.PlayerTransform.position,
                    context.speed * Time.deltaTime
                );
                elapsedTime += Time.deltaTime;
                await UniTask.Yield();
            }
        }
    }
