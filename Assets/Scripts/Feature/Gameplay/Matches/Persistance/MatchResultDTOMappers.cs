using UnityEngine;

public static class MatchResultDTOMappers
{
    public static MatchResultDTO ToDTO(this MatchResult result)
    {
        return new MatchResultDTO
        {
            matchId = result.MatchId.ToString(),
            winnerId = result.WinnerId.ToString(),
            endTime = result.EndTime.ToString("O")
        };
    }
}
