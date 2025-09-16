using System;
using System.Collections.Generic;

public class Enemy1 : EnemyBase
{
    void Start()
    {
        SetActions();
    }
    void SetActions()
    {
        Walk action1 = new Walk();
        base.actions.Add(action1);
        NullPo action2 = new NullPo();
        base.actions.Add(action2);
    }
}
