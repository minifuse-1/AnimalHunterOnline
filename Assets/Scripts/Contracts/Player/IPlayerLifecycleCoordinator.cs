using UnityEngine;

public interface IPlayerLifecycleCoordinator
{
    public IPlayerActor AddPlayer(PlayerProfile playerProfile);
    public bool RemovePlayer(PlayerId playerId);
}
