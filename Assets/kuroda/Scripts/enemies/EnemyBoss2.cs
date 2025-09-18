using System;
using System.Collections.Generic;
using kuroda.Scripts.actions;
using UnityEngine;

public class EnemyBoss2 : EnemyBase
{
    override protected void SetActions()
    {
    }
    override protected void OnPlayerHit()
    {
        // Debug.Log("敵ボス2が当たってるよ");
        // player_.OnDamage((int)attackPower);
    }
}
