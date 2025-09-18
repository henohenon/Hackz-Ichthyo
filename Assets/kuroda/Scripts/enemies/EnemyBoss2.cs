using System;
using UnityEngine;
using kuroda.Scripts.actions;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;

public class EnemyBoss2 : EnemyBase
{
    protected override void SetActions()
    {
        // NullPo を4回繰り返す（それぞれ0.3秒
        for (int i = 0; i < 4; i++)
        {
            actionDurationPairs.Add(new NullPo(), 0.3f);
        }

        for (int i = 0; i < 60; i++)
        {
            actionDurationPairs.Add(new RetreatFromPlayer(), 0.05f);
        }

        actionDurationPairs.Add(new WaitAction(), 4f);
    }

    private bool hitted = false;
    private float hitCoolDown = 0.5f;

    protected override void OnPlayerHit()
    {
        if (!gameObject.activeSelf || hitted) return;

        Debug.Log("EnemyBoss2がプレイヤーにヒット！");
        player_.OnDamage((int)attackPower);
        hitted = true;
        HitCoolDown().Forget();
    }

    private async Cysharp.Threading.Tasks.UniTaskVoid HitCoolDown()
    {
        await Cysharp.Threading.Tasks.UniTask.Delay(TimeSpan.FromSeconds(hitCoolDown));
        if (!gameObject.activeSelf) return;
        hitted = false;
    }
}