using System;
using System.IO;
using UnityEngine;

public sealed class BootstrapCompositionRoot : MonoBehaviour
{
    [SerializeField] private SceneFlowController _sceneFlowController;
    [SerializeField] private LoadingScreenController _loadingScreenController;
    [SerializeField] private ScenePathSO _mainMenuSO;
    [SerializeField] private ScenePathSO _loginSceneSO;
    [SerializeField] private NetworkSettingsSO _networkSettingsSO;
    private GameApp _app;
    private void Awake()
    {
        if (_sceneFlowController == null) throw new ArgumentNullException(nameof(_sceneFlowController));
        if (_loadingScreenController == null) throw new ArgumentNullException(nameof(_loadingScreenController));
        if (_mainMenuSO == null) throw new ArgumentNullException(nameof(_mainMenuSO));
        if (_loginSceneSO == null) throw new ArgumentNullException(nameof(_loginSceneSO));
        GameAppBuilder appBuilder = new();
        NodeClient nodeClient = new(_networkSettingsSO.BaseURL);
        IAuthenticationClient authenticationClient = nodeClient;
        IPlayerProfileClient profileClient = nodeClient;
        string authPath = Path.Combine(Application.persistentDataPath, AppConstants.Auth.DataPath);
        IAuthenticationSessionStore authenticationSessionStore = new JsonAuthenticationSessionStore(authPath);
        _app = appBuilder
        .Add(new SceneLoader())
        .Add(_sceneFlowController)
        .Add(_loadingScreenController)
        .Add(new StartupScenes(menuSO: _mainMenuSO, loginSO: _loginSceneSO))
        .Add(authenticationClient)
        .Add(profileClient)
        .Add(authenticationSessionStore)
        .Build();

        Debug.Log($"Authentication session path: {authPath}");
    }

    private async Awaitable Start()
    {
        if (_app == null) throw new ArgumentNullException(nameof(_app));

        await _app.StartAsync(destroyCancellationToken);
    }
}
