using System;
using TMPro;
using UnityEngine;

public sealed class LoginController : MonoBehaviour
{
    [SerializeField] private ScenePathSO _nextScene;
    [SerializeField] private TMP_InputField _emailInputField;
    [SerializeField] private TMP_InputField _passwordInputField;
    [SerializeField] private TMP_Text _statusText;

    private ISceneFlowController _sceneFlowController;
    private IAuthenticationService _authService;

    public void Bind(ISceneFlowController sceneFlowController, IAuthenticationService authenticationService)
    {
        _sceneFlowController = sceneFlowController ?? throw new ArgumentNullException(nameof(sceneFlowController));
        _authService = authenticationService ?? throw new ArgumentNullException(nameof(authenticationService));
    }

    public async void Login()
    {
        try
        {
            _statusText.color = Color.white;
            _statusText.text = "Logging in...";
            await _authService.LoginAsync(_emailInputField.text, _passwordInputField.text, destroyCancellationToken);
            destroyCancellationToken.ThrowIfCancellationRequested();
            _statusText.color = Color.green;
            _statusText.text = "Logged in!";
            _sceneFlowController.ChangePrimaryScene(_nextScene);
        }
        catch (Exception ex)
        {
            Debug.LogError(ex);
        }
    }
}
