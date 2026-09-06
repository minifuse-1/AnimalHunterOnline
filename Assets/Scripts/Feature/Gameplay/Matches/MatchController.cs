using System;
using UnityEngine;

public class MatchController : IMatchController
{
    public bool isCompleted => _isCompleted;

    public event Action<MatchResult> MatchCompleted;
    private bool _isCompleted;
    private MatchId _matchId;

    public MatchController(MatchId matchId)
    {
        if (matchId.Guid == Guid.Empty) throw new ArgumentException(nameof(matchId));
        
        _matchId = matchId;
    }

    public bool TryDeclareWinner(PlayerId playerId)
    {
        if (_isCompleted) return false;

        _isCompleted = true;

        MatchResult result = new(AppConstants.MatchHistory.FormatVersion, _matchId, playerId, DateTimeOffset.UtcNow);

        MatchCompleted?.Invoke(result);
        return true;
    }
}
