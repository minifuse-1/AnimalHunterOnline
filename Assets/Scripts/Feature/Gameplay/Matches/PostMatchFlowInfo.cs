using System;
using UnityEngine;

public readonly struct PostMatchFlowInfo
{
    public TimeSpan MinPostMatchDuration { get; }

    public PostMatchFlowInfo(TimeSpan minDuration)
    {
        MinPostMatchDuration = minDuration;
    }
}
