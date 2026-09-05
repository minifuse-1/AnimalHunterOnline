using UnityEngine;
using System;

public class MainMenuController : MonoBehaviour, ISceneNavActions
{
    private ISceneFlowController _sceneFlowController;
    [SerializeField] private ScenePathSO _gameplayPathSO;
    public void Bind(ISceneFlowController sceneFlowController)
    {
        _sceneFlowController = sceneFlowController ?? throw new ArgumentNullException(nameof(sceneFlowController));
    }

    public void StartGame()
    {
        _sceneFlowController.ChangePrimaryScene(_gameplayPathSO);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
