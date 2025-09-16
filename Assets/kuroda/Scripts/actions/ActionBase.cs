using System.Threading.Tasks;
using UnityEngine;
abstract public class ActionBase
{
    public abstract Task DoAction(Context context, float? duration = 0);
}
