using UnityEngine;

public class MainMenuSceneCompositionRoot : MonoBehaviour, ILoadedSceneCompositionRoot
{
    public void Initialize(AppDependencies appDependencies)
    {
        Debug.Log("Main Menu Scene Loaded");
    }
}
