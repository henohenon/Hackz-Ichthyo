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
        Debug.Log("NullPoするよ");
        // テキストをplayerに向けて発射する
        GameObject gameObject = CreateTextObject(context);
        if (gameObject == null)
        {
            return;
        }
        MoveNullObject(context, gameObject);
        return;
    }

    private GameObject CreateTextObject(Context context)
    {
        nullPoPrefab = Resources.Load<GameObject>("NullPoPrefab");
        if (nullPoPrefab == null)
        {
            Debug.LogError("Resourcesフォルダに'NullPoPrefab'が見つかりません");
            return null;
        }

        // canvasがあるかチェック
        Canvas canvas = Object.FindObjectOfType<Canvas>();
        if (canvas == null)
        {
            Debug.LogError("シーンにCanvasがありません");
            return null;
        }
        // canvasがあるかチェック
        // PrefabをCanvasの子要素として生成
        GameObject instance = Object.Instantiate(nullPoPrefab, canvas.transform);

        // 生成したオブジェクトの初期位置を敵の位置に設定
        instance.transform.position = context.EnemyTransform.position;

        return instance;
    }

    private async Task MoveNullObject(Context context, GameObject gameObject)
    {
        while (gameObject != null)
        {
            if (Vector3.Distance(gameObject.transform.position, context.PlayerTransform.position) < 0.1f)
            {
                break;
            }
            gameObject.transform.position = Vector3.MoveTowards(
                gameObject.transform.position,
                context.PlayerTransform.position,
                NullPoSpeed * Time.deltaTime
            );
            await UniTask.Yield();
        }
        if (gameObject != null)
        {
            Object.Destroy(gameObject);
        }
    }
}
