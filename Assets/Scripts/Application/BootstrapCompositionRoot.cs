using System;
using UnityEngine;

public sealed class BootstrapCompositionRoot : MonoBehaviour
{
    [SerializeField] private SceneFlowController _sceneFlowController;
    [SerializeField] private LoadingScreenController _loadingScreenController;
    [SerializeField] private ScenePathSO _pathSO;
    private GameApp _app;
    private void Awake()
    {
        if (_sceneFlowController == null) throw new ArgumentNullException(nameof(_sceneFlowController));
        if (_loadingScreenController == null) throw new ArgumentNullException(nameof(_loadingScreenController));
        GameAppBuilder appBuilder = new();
        appBuilder
        .Add(new SceneLoader())
        .Add(_sceneFlowController)
        .Add(_loadingScreenController)
        .Add(_pathSO)
        .Build();
        _app = appBuilder.Build();
    }

    private async Awaitable Start()
    {
        if (_app == null) throw new ArgumentNullException(nameof(_app));

        await _app.StartAsync(destroyCancellationToken);
    }
}
