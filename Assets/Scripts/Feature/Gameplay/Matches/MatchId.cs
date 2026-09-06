using System;

public readonly struct MatchId : IEquatable<MatchId>
{
    public Guid Guid { get; }

    public MatchId(Guid id)
    {
        if (Guid.Empty == id) throw new ArgumentException(nameof(id));

        Guid = id;
    }
    public bool Equals(MatchId other)
    {
        return Guid.Equals(Guid, other.Guid);
    }

    public override bool Equals(object obj)
    {
        return obj is MatchId other && Equals(other);
    }

    public override int GetHashCode()
    {
        return Guid.GetHashCode();
    }

    public override string ToString()
    {
        return Guid.ToString();
    }

    public static bool operator ==(MatchId left, MatchId right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(MatchId left, MatchId right)
    {
        return !left.Equals(right);
    }
}
