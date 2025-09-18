using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class Enemy1 : EnemyBase
{
    override protected void SetActions()
    {
        Walk action1 = new ();
        actionDurationPairs.Add(action1, 5);
        NullPo nullPo = new ();
        actionDurationPairs.Add(nullPo, 5);
    }

    private bool hitted = false;
    private float hitCoolDown = 0.5f;
    override protected void OnPlayerHit()
    {
        if (!gameObject.activeSelf) return;
        if (hitted) return;
        Debug.Log("敵1が当たってるよ");
        player_.OnDamage((int)attackPower);
        hitted = true;
        HitCoolDown().Forget();
        // Destroy(this.gameObject);
    }

    private async UniTaskVoid HitCoolDown()
    {
        await UniTask.Delay(TimeSpan.FromSeconds(hitCoolDown));
        if (!gameObject.activeSelf) return;
        hitted = false;
    }
}
