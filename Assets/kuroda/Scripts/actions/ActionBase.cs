using System;
using System.Threading.Tasks;
using UnityEngine;
abstract public class ActionBase: IDisposable
{
    public abstract Task DoAction(Context context, float? duration = 0);
    
    public virtual void Dispose()
    {
    }
}
