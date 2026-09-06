using System;

public sealed class GameAppBuilder
{
    private SceneFlowController _sceneFlowController;
    private ISceneLoader _sceneLoader;
    private LoadingScreenController _loadingScreenController;
    private IAuthenticationClient _authClient;
    private IAuthenticationSessionStore _authSessionStore;
    private IPlayerProfileClient _profileClient;
    private StartupScenes _startupScenes;
    private IMatchResultSink _matchResultSink;

    public void RegisterMatchResultSink(IMatchResultSink matchResultSink)
    {
        if (_matchResultSink != null)
            throw new InvalidOperationException(nameof(RegisterMatchResultSink));

        _matchResultSink = matchResultSink;
    }
    
    public void RegisterAuthenticationClient(IAuthenticationClient authClient)
    {
        if (_authClient != null) 
            throw new InvalidOperationException(nameof(RegisterAuthenticationClient));

        _authClient = authClient;
    }

    public void RegisterAuthenticationSessionStore(IAuthenticationSessionStore authenticationSessionStore)
    {
        if (_authSessionStore != null)
            throw new InvalidOperationException(nameof(RegisterAuthenticationSessionStore));

        _authSessionStore = authenticationSessionStore;
    }

    public void RegisterPlayerProfileClient(IPlayerProfileClient playerProfileClient)
    {
        if (_profileClient != null)
            throw new InvalidOperationException(nameof(RegisterPlayerProfileClient));

        _profileClient = playerProfileClient;
    }

    public void RegisterStartupScenes(StartupScenes scenes)
    {
        if (scenes.LoginSceneSO == null || scenes.MainMenuSO == null) throw new ArgumentNullException(nameof(scenes));
        _startupScenes = scenes;
    }

    public void RegisterSceneFlowController(SceneFlowController sceneFlowController)
    {
        if (_sceneFlowController != null) throw new InvalidOperationException(nameof(RegisterSceneFlowController));

        _sceneFlowController = sceneFlowController; 
    }
    public void RegisterSceneLoader(ISceneLoader sceneLoader)
    {
        if (_sceneLoader != null) throw new InvalidOperationException(nameof(RegisterSceneLoader));

        _sceneLoader = sceneLoader;
    }
    public void RegisterLoadingScreenController(LoadingScreenController loadingScreenController)
    {
        if (_loadingScreenController != null) throw new InvalidOperationException(nameof(RegisterLoadingScreenController));
        _loadingScreenController = loadingScreenController;
    }
    public GameApp Build()
    {
        if (!HasValidRefs())
            throw new InvalidOperationException(nameof(Build));

        PlayerProfileService profileService = new();
        AuthenticationService authenticationService = new(_authClient, _profileClient, _authSessionStore, profileService);
        GameplaySessionService gameplaySessionService = new();
        _sceneFlowController.Bind(_sceneLoader);
        _loadingScreenController.Bind(_sceneFlowController);
        AppDependencies appDependencies = new(
            _sceneFlowController, 
            authenticationService, 
            profileService, 
            _matchResultSink, 
            gameplaySessionService
            );
        
        _sceneFlowController.Initialize(appDependencies);

        return new GameApp(_sceneFlowController, authenticationService, _startupScenes);
    }

    private bool HasValidRefs()
    {
        return 
            _sceneFlowController != null 
            && _sceneLoader != null 
            && _loadingScreenController != null 
            && _authClient != null 
            && _authSessionStore != null 
            && _profileClient != null
            && _matchResultSink != null;
    }
}
