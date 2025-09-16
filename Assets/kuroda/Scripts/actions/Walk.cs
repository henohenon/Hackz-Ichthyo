using UnityEngine;

public class Walk : ActionBase
{

    public override void DoAction(Context context)
    {
        context.EnemyTransform.position = Vector3.MoveTowards(
            context.EnemyTransform.position,
            context.PlayerTransform.position,
            context.speed * Time.deltaTime
        );
    }
}
