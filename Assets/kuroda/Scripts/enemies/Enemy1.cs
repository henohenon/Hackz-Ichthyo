using System;
using System.Collections.Generic;

public class Enemy1 : EnemyBase
{

    override protected void SetActions()
    {
        Walk action1 = new Walk();
        actionDurationPairs.Add(action1, 5);
        NullPo nullPo = new NullPo();
        actionDurationPairs.Add(nullPo, 5);
    }

}
