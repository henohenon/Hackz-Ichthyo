using System;
using System.Collections.Generic;
using UnityEngine;

public class Enemy1 : EnemyBase
{

    override protected void SetActions()
    {
        Walk action1 = new Walk();
        actionDurationPairs.Add(action1, 5);
        NullPo nullPo = new NullPo();
        actionDurationPairs.Add(nullPo, 5);
    }
    override protected void OnPlayerHit()
    {
        Debug.Log("敵1が当たってるよ");
        player_.OnDamage((int)attackPower);
        Destroy(this.gameObject);
    }
}
