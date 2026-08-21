using System;

public readonly struct PlayerId : IEquatable<PlayerId>
{
    public Guid Guid {get;}
    public PlayerId(Guid guid)
    {
        if (guid == Guid.Empty) throw new ArgumentException(nameof(guid));
        Guid = guid;
    }

    public bool Equals(PlayerId other)
    {
        return Guid.Equals(other.Guid);
    }

    public override bool Equals(object obj)
    {
        return obj is PlayerId other && Equals(other);
    }

    public static bool operator ==(PlayerId left, PlayerId right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(PlayerId left, PlayerId right)
    {
        return !left.Equals(right);
    }

    public override string ToString()
    {
        return Guid.ToString();
    }

    public override int GetHashCode()
    {
        return Guid.GetHashCode();
    }
}