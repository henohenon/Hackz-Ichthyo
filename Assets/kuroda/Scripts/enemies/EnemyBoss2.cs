using System;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBoss2 : EnemyBase
{
    override protected void SetActions()
    {
        Walk action1 = new Walk();
        actionDurationPairs.Add(action1, 5);
        Stop stop = new Stop();
        actionDurationPairs.Add(stop, null);
        //USBSord uSBSord = new USBSord();
        //actionDurationPairs.Add(uSBSord, null);
    }
    override protected void OnPlayerHit()
    {
        // Debug.Log("敵ボス2が当たってるよ");
        // player_.OnDamage((int)attackPower);
    }
}
