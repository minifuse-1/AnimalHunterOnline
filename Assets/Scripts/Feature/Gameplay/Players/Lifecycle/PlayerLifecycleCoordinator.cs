using UnityEngine;
using System;
using System.Collections.Generic;

public class PlayerLifecycleCoordinator : IPlayerLifecycleCoordinator
{
    private readonly IPlayerSpawner _playerSpawner;
    private readonly SpawnPointSet _spawnPoints;

    public PlayerLifecycleCoordinator(IPlayerSpawner playerSpawner, SpawnPointSet spawnPoints)
    {
        _playerSpawner = playerSpawner ?? throw new ArgumentNullException(nameof(playerSpawner));
        _spawnPoints = spawnPoints;
    }

    public IPlayerActor AddPlayer(PlayerProfile playerProfile)
    {
        int index = UnityEngine.Random.Range(0, _spawnPoints.Points.Count);
        Transform spawn = _spawnPoints.Points[index];
        return _playerSpawner.Spawn(playerProfile, spawn.position, spawn.rotation);
    }

    public bool RemovePlayer(PlayerId playerId)
    {
        return _playerSpawner.Despawn(playerId);
    }
}
