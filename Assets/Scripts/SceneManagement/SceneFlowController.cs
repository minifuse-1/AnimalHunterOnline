using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public sealed class SceneFlowController : MonoBehaviour, ISceneFlowController, ISceneTransitionEvents
{
    public event Action TransitionStarted;
    public event Action TransitionCompleted;
    public event Action<float> ProgressChanged;
    private ISceneLoader _sceneLoader;
    private AppDependencies? _appDependencies;
    private Scene _primaryScene;
    private readonly List<Scene> _secondaryScenes = new();
    private bool _isTransitioning;
    public void Initialize(AppDependencies dependencies)
    {
        _appDependencies = dependencies;
    }

    public void Bind(ISceneLoader sceneLoader)
    {
        _sceneLoader = sceneLoader ?? throw new ArgumentNullException(nameof(sceneLoader));
    }

    public void AddScene(ScenePathSO pathSO)
    {
        StartCoroutine(AddSceneCoroutine(pathSO));
    }

    private IEnumerator AddSceneCoroutine(ScenePathSO pathSO)
    {
        AsyncOperation loadingOperation = _sceneLoader.loadAdditivelyAsync(pathSO);
        yield return loadingOperation;

        if (!_sceneLoader.TryGetLoadedScene(pathSO, out Scene scene))
            yield break;

        InitializeScene(scene);
        _secondaryScenes.Add(scene);
    }

    public void ChangePrimaryScene(ScenePathSO pathSO)
    {
        if (_isTransitioning) return;

        _isTransitioning = true;

        StartCoroutine(ChangePrimarySceneCoroutine(pathSO));
    }

    private IEnumerator ChangePrimarySceneCoroutine(ScenePathSO pathSO)
    {
        TransitionStarted?.Invoke();
        ProgressChanged?.Invoke(0f);

        Scene previousPrimaryScene = _primaryScene;
        List<Scene> prevSecondaryScenes = new(_secondaryScenes);

        AsyncOperation loadingOperation = _sceneLoader.loadAdditivelyAsync(pathSO);
        yield return TrackProgressCoroutine(loadingOperation, 0f, 0.8f);

        if (!_sceneLoader.TryGetLoadedScene(pathSO, out Scene scene))
        {
            _isTransitioning = false;
            TransitionCompleted?.Invoke();
            Debug.LogError($"Could not load Scene with path: {pathSO.Path}");
            yield break;
        }

        _secondaryScenes.Clear();
        _sceneLoader.SetActive(scene);
        InitializeScene(scene);
        _primaryScene = scene;

        foreach (Scene secondaryScene in prevSecondaryScenes)
        {
            if (secondaryScene.IsValid() && secondaryScene.isLoaded)
                yield return _sceneLoader.unloadAsync(secondaryScene);
        }

        if (previousPrimaryScene.IsValid() && previousPrimaryScene.isLoaded)
        {
            AsyncOperation unloadingOperation = _sceneLoader.unloadAsync(previousPrimaryScene);
            yield return TrackProgressCoroutine(unloadingOperation, 0.8f, 1f);
        }

        ProgressChanged?.Invoke(1f);
        TransitionCompleted?.Invoke();
        _isTransitioning = false;
    }

    private IEnumerator TrackProgressCoroutine(AsyncOperation operation, float start, float end)
    {
        while (!operation.isDone)
        {
            float operationProg = Mathf.Clamp01(operation.progress);
            float transitionProg = Mathf.Lerp(start, end, operationProg);
            ProgressChanged?.Invoke(transitionProg);
            yield return null;
        }

        ProgressChanged?.Invoke(end);
    }
    public void RemoveScene(ScenePathSO pathSO)
    {
        if (!_sceneLoader.TryGetLoadedScene(pathSO, out Scene scene)) return;

        RemoveScene(scene);
    }

    public void RemoveScene(Scene scene)
    {
        StartCoroutine(RemoveSceneCoroutine(scene));
    }

    private IEnumerator RemoveSceneCoroutine(Scene scene)
    {
        if (scene.IsValid() && scene.isLoaded) yield return _sceneLoader.unloadAsync(scene);

        _secondaryScenes.Remove(scene);
    }

    private void InitializeScene(Scene scene)
    {
        if (!_appDependencies.HasValue) throw new InvalidOperationException(nameof(InitializeScene));

        GameObject[] rootObjects = scene.GetRootGameObjects();

        if (rootObjects.Length != 1) throw new InvalidOperationException(nameof(InitializeScene));

        if (!rootObjects[0].TryGetComponent(out ILoadedSceneCompositionRoot root)) throw new InvalidOperationException(nameof(InitializeScene));

        root.Initialize(_appDependencies.Value);
    }
}
