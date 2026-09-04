using System;
using UnityEngine;

public sealed class LoginCompositionRoot : MonoBehaviour, ILoadedSceneCompositionRoot
{

    [SerializeField] private LoginController _loginController;
    public void Initialize(AppDependencies appDependencies)
    {
        if (_loginController == null) throw new ArgumentException(nameof(_loginController));

        _loginController.Bind(appDependencies.SceneFlowController, appDependencies.AuthenticationService);
    }
}
