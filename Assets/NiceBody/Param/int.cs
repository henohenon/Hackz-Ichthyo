using System;

/// <summary>
/// NOTE: 値オブジェクト推奨
/// </summary>
[Serializable]
public struct @int
{
    [UnityEngine.SerializeField] private int value_;

    public @int(int value)
    {
        value_ = value;
    }

    public readonly int Value => value_;
}