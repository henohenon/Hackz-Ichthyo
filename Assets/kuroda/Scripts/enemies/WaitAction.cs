using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class WaitAction : ActionBase
{
    public override async Task DoAction(Context context, float? duration = 0)
    {
        await UniTask.WaitForSeconds((float)duration);
    }
}
