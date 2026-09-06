
using System;

public readonly struct AppDependencies
{
    public ISceneFlowController SceneFlowController {get;}
    public IAuthenticationService AuthenticationService {get;}
    public IPlayerProfileContext PlayerProfileContext {get;}
    public IMatchResultSink MatchResultSink { get;}
    public IGameplaySessionService GameplaySessionService { get; }
    public AppDependencies(
        ISceneFlowController sceneFlowController, 
        IAuthenticationService authenticationService, 
        IPlayerProfileContext playerProfileContext, 
        IMatchResultSink matchResultSink, 
        IGameplaySessionService gameplaySessionService
        )
    {
        SceneFlowController = sceneFlowController ?? throw new ArgumentNullException(nameof(sceneFlowController));
        AuthenticationService = authenticationService ?? throw new ArgumentNullException(nameof(authenticationService));
        PlayerProfileContext = playerProfileContext ?? throw new ArgumentNullException(nameof(playerProfileContext));
        MatchResultSink = matchResultSink ?? throw new ArgumentNullException(nameof(matchResultSink));
        GameplaySessionService = gameplaySessionService ?? throw new ArgumentNullException(nameof(gameplaySessionService));
    }
}
