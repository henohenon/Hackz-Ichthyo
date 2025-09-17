using System;
using System.Collections.Generic;
using UnityEngine;

public class Enemy2 : EnemyBase
{
    private static int counter = 0;

    override protected void SetActions()
    {
        Walk action1 = new Walk();
        actionDurationPairs.Add(action1, 5);
    }
    override protected void OnPlayerHit()
    {
        Debug.Log("敵2が当たってるよ");
        player_.OnDamage((int)attackPower);
        Destroy(this.gameObject);
    }
    void OnDestroy()
    {
        counter++;
    }
    public int getNumber()
    {
        return counter;
    }
}
