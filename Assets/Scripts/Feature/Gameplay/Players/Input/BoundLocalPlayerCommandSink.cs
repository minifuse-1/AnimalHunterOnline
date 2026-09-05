using System;
using UnityEngine;

public class BoundLocalPlayerCommandSink : IPlayerCommandSink
{
    private readonly IPlayerCommandDispatcher _dispatcher;
    private readonly PlayerId _playerId;

    public BoundLocalPlayerCommandSink(IPlayerCommandDispatcher dispatcher, PlayerId playerId)
    {
        _dispatcher = dispatcher ?? throw new ArgumentNullException(nameof(dispatcher));
        _playerId = playerId;
    }
    public void Submit(in PlayerCommand playerCommand)
    {
        if (!_dispatcher.Dispatch(_playerId, playerCommand))
            Debug.LogWarning($"No player with playerID {_playerId} in registry");
    }
}
