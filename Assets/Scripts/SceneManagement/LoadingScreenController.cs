using System;
using UnityEngine;
using UnityEngine.UI;

public sealed class LoadingScreenController : MonoBehaviour
{
    [SerializeField] private GameObject _loadingCanvas;
    [SerializeField] private Slider _loadingBarSlider;
    private ISceneTransitionEvents _sceneTransitionEvents;

    public void Bind(ISceneTransitionEvents sceneTransitionEvents)
    {
        if (ReferenceEquals(_sceneTransitionEvents, sceneTransitionEvents)) return;
        Unbind();
        _sceneTransitionEvents = sceneTransitionEvents;
        if (_sceneTransitionEvents == null) return;
        _sceneTransitionEvents.TransitionStarted += EnableCanvas;
        _sceneTransitionEvents.TransitionCompleted += DisableCanvas;
        _sceneTransitionEvents.ProgressChanged += UpdateLoadingBarProgress;
    }

    public void Unbind()
    {
        if (_sceneTransitionEvents == null) return;

        _sceneTransitionEvents.TransitionStarted -= EnableCanvas;
        _sceneTransitionEvents.TransitionCompleted -= DisableCanvas;
        _sceneTransitionEvents.ProgressChanged -= UpdateLoadingBarProgress;
    }

    private void EnableCanvas()
    {
        _loadingCanvas.SetActive(true);
    }

    private void DisableCanvas()
    {
        _loadingCanvas.SetActive(false);
    }

    private void UpdateLoadingBarProgress(float currentProgress)
    {
        _loadingBarSlider.value = Mathf.Clamp01(currentProgress);
    }
}
