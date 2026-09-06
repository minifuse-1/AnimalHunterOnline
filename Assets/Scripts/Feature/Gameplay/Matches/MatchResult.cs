using System;
using UnityEngine;

public readonly struct MatchResult
{
    public string Version { get; }
    public MatchId MatchId { get; }
    public PlayerId WinnerId { get; }
    public DateTimeOffset EndTime { get; }

    public MatchResult(string version, MatchId matchId, PlayerId winnerId, DateTimeOffset endTime)
    {
        Version = version;
        MatchId = matchId;
        WinnerId = winnerId;
        EndTime = endTime;
    }
}