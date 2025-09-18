using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using kuroda.Scripts.actions;

public class EnemyBoss2 : EnemyBase
{
    protected override void SetActions()
    {
        var nullPo = new NullPo();
        actionDurationPairs.Add(nullPo, null);

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
