using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class Enemy2 : EnemyBase
{
    override protected void SetActions()
    {
        Walk action1 = new Walk();
        actionDurationPairs.Add(action1, 5);
    }
    private bool hitted = false;
    private float hitCoolDown = 0.5f;
    override protected void OnPlayerHit()
    {
        if (!gameObject.activeSelf) return;
        if (hitted) return;
        hitted = true;
        player_.OnDamage((int)attackPower);
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
