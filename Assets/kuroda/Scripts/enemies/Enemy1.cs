using System;
using System.Collections.Generic;

public class Enemy1 : EnemyBase
{

    override protected void SetActions()
    {
        Walk action1 = new Walk();
        actionDurationPairs.Add(action1, 5);
        Stop stop = new Stop();
        actionDurationPairs.Add(stop, 5);
        Teleport teleport = new Teleport();
        actionDurationPairs.Add(teleport, 5);
    }

}
