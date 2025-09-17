using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NullPo : ActionBase
{
    private GameObject nullPoPrefab;
    private float NullPoSpeed = 2f;
    public override async Task DoAction(Context context, float? duration)
    {
        nullPoPrefab = Resources.Load<GameObject>("NullPoPrefab");
        if (nullPoPrefab == null)
        {
            Debug.LogError("Resourcesフォルダに'NullPoPrefab'が見つかりません");
            return;
        }
        GameObject instance = Object.Instantiate(nullPoPrefab, context.EnemyTransform.position, Quaternion.identity);

        // 生成したオブジェクトにアタッチされているNullPoProjectileスクリプトを取得
        NullPoObject nullPoObject = instance.GetComponent<NullPoObject>();
        if (nullPoObject != null)
        {
            Vector2 direction = context.PlayerTransform.position - context.EnemyTransform.position;
            nullPoObject.Launch(direction);
        }
    }
}
