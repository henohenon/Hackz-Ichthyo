using System.Collections.Generic;
using UnityEditor.Rendering.LookDev;
using UnityEngine;

abstract public class EnemyBase : MonoBehaviour
{
    [SerializeField] protected int hitPoint, speed;
    protected List<ActionBase> actions = new List<ActionBase>();

    protected void OnAction()
    {
        Context context = SetContext();
        foreach (ActionBase action in actions)
        {
            action.DoAction(context);
        }
    }
    protected Context SetContext()
    {
        Context context = new Context();
        context.EnemyTransform = this.transform;
        context.PlayerTransform = this.transform;//仮置き、後で変更する
        context.speed = this.speed;
        return context;
    }
}
