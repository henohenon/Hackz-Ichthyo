using System;
using System.Collections.Generic;

public class Enemy2 : EnemyBase
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

}
