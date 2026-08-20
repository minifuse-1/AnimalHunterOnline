using UnityEngine;
using UnityEngine.SceneManagement;

public sealed class SceneLoader : ISceneLoader
{
    public AsyncOperation loadAdditivelyAsync(ScenePathSO pathSO)
    {
        return SceneManager.LoadSceneAsync(pathSO.Path, LoadSceneMode.Additive);
    }

    public void SetActive(Scene scene)
    {
        SceneManager.SetActiveScene(scene);
    }

    public bool TryGetLoadedScene(ScenePathSO pathSO, out Scene scene)
    {
        scene = SceneManager.GetSceneByPath(pathSO.Path);
        return scene.IsValid() && scene.isLoaded;
    }

    public AsyncOperation unloadAsync(Scene scene)
    {
        return SceneManager.UnloadSceneAsync(scene);
    }
}
