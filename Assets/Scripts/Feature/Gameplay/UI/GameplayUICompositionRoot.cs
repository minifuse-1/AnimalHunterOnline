using System;
using TMPro;
using UnityEngine;

public class GameplayUICompositionRoot : MonoBehaviour, ILoadedSceneCompositionRoot
{
    [SerializeField] private GameplayUIController _gameplayUIController;
    public void Initialize(AppDependencies appDependencies)
    {
        if (_gameplayUIController == null) throw new ArgumentNullException(nameof(_gameplayUIController));
        IPlayerProfileContext profileContext = appDependencies.PlayerProfileContext ?? throw new ArgumentNullException(nameof(appDependencies.PlayerProfileContext));

        if (!profileContext.HasProfile) throw new InvalidOperationException(nameof(Initialize));

        if (!appDependencies.GameplaySessionService.TryGetSessionContext(out IGameplaySessionContext gameplaySessionContext)) throw new InvalidOperationException("No Active gameplay session");

        if (gameplaySessionContext is not IPostMatchFlowContext postMatchFlowContext) throw new InvalidOperationException("Gameplay sessioncontext does not provide definition for PostMatchFlowContext!");

        _gameplayUIController.Bind(profileContext, gameplaySessionContext.MatchResultSource, postMatchFlowContext);
    }
}
