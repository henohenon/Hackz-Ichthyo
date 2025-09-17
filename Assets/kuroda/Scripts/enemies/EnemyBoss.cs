using System;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBoss : EnemyBase
{
    override protected void SetActions()
    {
        Walk action1 = new Walk();
        actionDurationPairs.Add(action1, 5);
        Stop stop = new Stop();
        actionDurationPairs.Add(stop, null);
        Teleport teleport = new Teleport();
        actionDurationPairs.Add(teleport, null);
    }
    override protected void OnPlayerHit()
    {
        Debug.Log("敵ボスが当たってるよ");
        player_.OnDamage((int)attackPower);
    }
}
