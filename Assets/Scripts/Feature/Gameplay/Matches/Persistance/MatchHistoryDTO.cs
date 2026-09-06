using System.Collections.Generic;
using System;
using UnityEngine;

[Serializable]
public sealed class MatchHistoryDTO
{
    public string version;
    public List<MatchResultDTO> results;
}
