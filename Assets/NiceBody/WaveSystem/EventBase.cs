using Cysharp.Threading.Tasks;
using UnityEngine;

public abstract class EventBase : ScriptableObject
{
    public abstract UniTask ExecuteAsync(WaveSystem waveSystem);
}