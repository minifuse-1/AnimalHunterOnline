using System;
using Unity.VisualScripting;

public sealed class GameAppBuilder
{
    private SceneFlowController _sceneFlowController;
    private ISceneLoader _sceneLoader;
    private LoadingScreenController _loadingScreenController;
    private ScenePathSO _pathSO;

    public void RegisterScenePathSO(ScenePathSO pathSO)
    {
        if (_pathSO != null) throw new InvalidOperationException(nameof(RegisterScenePathSO));

        _pathSO = pathSO;
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

        _sceneFlowController.Bind(_sceneLoader);
        _loadingScreenController.Bind(_sceneFlowController);
        AppDependencies appDependencies = new(_sceneFlowController);
        
        _sceneFlowController.Initialize(appDependencies);

        return new GameApp(_sceneFlowController, _pathSO);
    }

    private bool HasValidRefs()
    {
        return _sceneFlowController != null && _sceneLoader != null && _loadingScreenController != null && _pathSO != null;
    }
}
