using System;
using UnityEngine;

public class PlayerSpawner : IPlayerSpawner
{
    private readonly IPlayerRegistry _registry;
    private readonly PlayerActor _playerPrefab;

    public PlayerSpawner(PlayerActor prefab, IPlayerRegistry registry)
    {
        _registry = registry ?? throw new ArgumentNullException(nameof(registry));
        _playerPrefab = prefab != null ? prefab : throw new ArgumentNullException(nameof(prefab));
    }

    public bool Despawn(PlayerId playerId)
    {
        if (!_registry.TryUnregister(playerId, out IPlayerActor playerActor)) return false;

        UnityEngine.Object.Destroy(playerActor.RootObject);
        return true;
    }

    public IPlayerActor Spawn(PlayerProfile playerProfile, Vector3 position, Quaternion rotation)
    {
        PlayerActor player = UnityEngine.Object.Instantiate(_playerPrefab, position, rotation);
        player.Initialize(playerProfile);

        if (!_registry.Register(player))
        {
            UnityEngine.Object.Destroy(player.RootObject);
            throw new InvalidOperationException(nameof(Spawn));
        }

        return player;
    }
}
