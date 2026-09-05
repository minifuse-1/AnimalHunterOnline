using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRegistry : IPlayerRegistry
{
    private readonly Dictionary<PlayerId, IPlayerActor> _players = new();
    public IReadOnlyCollection<IPlayerActor> PlayerActors => _players.Values;

    public bool Register(IPlayerActor playerActor)
    {
        if (playerActor == null) throw new ArgumentNullException(nameof(playerActor));

        return _players.TryAdd(playerActor.PlayerId, playerActor);
    }

    public bool TryGet(PlayerId playerId, out IPlayerActor playerActor)
    {
        return _players.TryGetValue(playerId, out playerActor);
    }

    public bool TryUnregister(PlayerId playerId, out IPlayerActor playerActor)
    {
        if (!_players.TryGetValue(playerId, out playerActor)) return false;

        return _players.Remove(playerId);
    }
}
