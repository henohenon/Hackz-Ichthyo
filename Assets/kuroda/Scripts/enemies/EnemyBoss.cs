using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using kuroda.Scripts.actions;
using UnityEngine;

public class EnemyBoss : EnemyBase
{
    protected override void SetActions()
    {
        var turnToTarget = new TurnToTarget();
        actionDurationPairs.Add(turnToTarget, 0.5f);
        var dash = new Dash();
        actionDurationPairs.Add(dash, 1);
    }
    private bool _hitted = false;
    private readonly float _hitCoolDown = 0.2f;
    protected override void OnPlayerHit()
    {
        if (_hitted) return;
        player_.OnDamage((int)attackPower);
        _hitted = true;
        HitCoolDown().Forget();
        // Destroy(this.gameObject);
    }

    private async UniTaskVoid HitCoolDown()
    {
        await UniTask.Delay(TimeSpan.FromSeconds(_hitCoolDown));
        if (!gameObject.activeSelf) return;
        _hitted = false;
    }
}
