using UnityEngine;

public interface IMatchController : IMatchResultSource
{
    public bool TryDeclareWinner(PlayerId playerId); // If winner isn't present?
}
