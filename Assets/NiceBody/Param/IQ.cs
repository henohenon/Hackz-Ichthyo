using System;

/// <summary>
/// NOTE: 値オブジェクト推奨
/// </summary>
[Serializable]
public struct IQ : IComparable<IQ>, IEquatable<IQ>
{
    [UnityEngine.SerializeField] private float value_;

    public IQ(float value)
    {
        value_ = value;
    }

    public readonly float Value => value_;

    public static IQ operator +(IQ a, IQ b   ) => new(a.value_ + b.value_);
    public static IQ operator -(IQ a, IQ b   ) => new(a.value_ - b.value_);
    public static IQ operator +(IQ a, float b) => new(a.value_ + b);
    public static IQ operator -(IQ a, float b) => new(a.value_ - b);

    public static bool operator ==(IQ a, IQ b) => a.value_ == b.value_;
    public static bool operator !=(IQ a, IQ b) => a.value_ != b.value_;
    public static bool operator < (IQ a, IQ b) => a.value_ < b.value_;
    public static bool operator > (IQ a, IQ b) => a.value_ > b.value_;
    public static bool operator <=(IQ a, IQ b) => a.value_ <= b.value_;
    public static bool operator >=(IQ a, IQ b) => a.value_ >= b.value_;

    public bool             Equals(IQ other)    => value_ == other.value_;
    public int              CompareTo(IQ other) => value_.CompareTo(other.value_);
    public override bool    Equals(object obj)  => obj is IQ other && Equals(other);
    public override int     GetHashCode()       => value_.GetHashCode();
}