using System;
using System.Threading;
using System.Threading.Tasks;
public sealed class GameApp
{
    private readonly ISceneFlowController _sceneFlowController;
    private readonly IAuthenticationService _authenticationService;
    private readonly StartupScenes _startupScenes;
    private bool _hasStarted;

    public GameApp(ISceneFlowController sceneFlowController, IAuthenticationService authenticationService, StartupScenes startupScenes)
    {
        _sceneFlowController = sceneFlowController ?? throw new ArgumentNullException(nameof(sceneFlowController));

        _authenticationService = authenticationService ?? throw new ArgumentNullException(nameof(authenticationService));
        _startupScenes = startupScenes;
    }

    public async Task StartAsync(CancellationToken ct = default)
    {
        if (_hasStarted) return;

        _hasStarted = true;
        bool sessionRestored = await _authenticationService.TryRestoreSessionAsync(ct);
        ct.ThrowIfCancellationRequested();
        ScenePathSO startScene = sessionRestored ? _startupScenes.MainMenuSO: _startupScenes.LoginSceneSO;
        _sceneFlowController.ChangePrimaryScene(startScene);

        return;
    }
}
