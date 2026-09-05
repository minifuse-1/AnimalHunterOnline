using UnityEngine;

public class GameplayUICompositionRoot : MonoBehaviour, ILoadedSceneCompositionRoot
{
    public void Initialize(AppDependencies appDependencies)
    {
        Debug.Log("UI Loaded");
    }
}
