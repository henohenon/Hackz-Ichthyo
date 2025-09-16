using System;

/// <summary>
/// NOTE: 値オブジェクト推奨
/// </summary>
[Serializable]
public struct Health
{
    [UnityEngine.SerializeField] private int value_;

    public Health(int value)
    {
        value_ = value;
    }
}