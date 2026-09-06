using System;
using UnityEngine;

public interface IPostMatchFlowSource
{
    public event Action<PostMatchFlowInfo> PostMatchStarted;
}
