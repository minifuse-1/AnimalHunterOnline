using System;
using UnityEngine;

public interface IMatchResultSource
{
    public event Action<MatchResult> MatchCompleted;
    public bool isCompleted { get; }
}
