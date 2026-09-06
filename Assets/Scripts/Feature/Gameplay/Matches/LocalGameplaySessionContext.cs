using System;
using UnityEngine;

public sealed class LocalGameplaySessionContext : IGameplaySessionContext, IPostMatchFlowContext
{
    public IPostMatchFlowSource PostMatchFlowSource { get; }

    public IMatchResultSource MatchResultSource { get; }

    public LocalGameplaySessionContext(IPostMatchFlowSource postMatchFlowSource, IMatchResultSource matchResultSource)
    {
        PostMatchFlowSource = postMatchFlowSource ?? throw new ArgumentNullException(nameof(postMatchFlowSource));
        MatchResultSource = matchResultSource ?? throw new ArgumentNullException(nameof(matchResultSource));
    }
}
