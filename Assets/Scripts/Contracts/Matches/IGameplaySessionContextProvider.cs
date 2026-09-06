using UnityEngine;

public interface IGameplaySessionContextProvider
{
    public bool TryGetSessionContext(out IGameplaySessionContext context);
}
