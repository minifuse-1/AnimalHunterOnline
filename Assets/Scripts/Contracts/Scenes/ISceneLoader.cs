using UnityEngine;
using UnityEngine.SceneManagement;

public interface ISceneLoader
{
    public AsyncOperation loadAdditivelyAsync(ScenePathSO pathSO);
    public AsyncOperation unloadAsync(Scene scene);

    public void SetActive(Scene scene);
    public bool TryGetLoadedScene(ScenePathSO pathSO, out Scene scene);
}
