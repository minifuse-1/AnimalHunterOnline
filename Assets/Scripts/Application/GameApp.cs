using System;
using System.Threading;
using System.Threading.Tasks;
public sealed class GameApp
{
    private readonly ISceneFlowController _sceneFlowController;
    private ScenePathSO _pathSO;
    private bool _hasStarted;

    public GameApp(ISceneFlowController sceneFlowController, ScenePathSO pathSO)
    {
        _sceneFlowController = sceneFlowController ?? throw new ArgumentNullException(nameof(sceneFlowController));

        _pathSO = pathSO ?? throw new ArgumentNullException(nameof(pathSO));
    }

    public Task StartAsync(CancellationToken ct = default)
    {
        if (_hasStarted) return Task.CompletedTask;

        _hasStarted = true;
        ct.ThrowIfCancellationRequested();

        _sceneFlowController.ChangePrimaryScene(_pathSO);

        return Task.CompletedTask;
    }
}
