using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class EnemyBoss : EnemyBase
{
    override protected void SetActions()
    {
        Walk action1 = new Walk();
        actionDurationPairs.Add(action1, 5);
        Stop stop = new Stop();
        actionDurationPairs.Add(stop, null);
        //Teleport teleport = new Teleport();
        //actionDurationPairs.Add(teleport, null);
    }
    private bool hitted = false;
    private float hitCoolDown = 0.2f;
    override protected void OnPlayerHit()
    {
        if (hitted) return;
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
