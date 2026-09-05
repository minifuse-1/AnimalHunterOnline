using System.Collections.Generic;
using System;
using UnityEngine;
using System.Linq;

public class MainMenuCompositionRoot : MonoBehaviour, ILoadedSceneCompositionRoot
{
    public void Initialize(AppDependencies dependencies)
    {
        if (dependencies.SceneFlowController is null)
        {
            throw new ArgumentNullException(nameof(dependencies.SceneFlowController));
        }

        // Bind all scene nav actions
        List<ISceneNavActions> navActions = GetComponentsInChildren<ISceneNavActions>().ToList();
        navActions.ForEach(action => action.Bind(dependencies.SceneFlowController));
    }
}
