
using System;

public readonly struct AppDependencies
{
    public ISceneFlowController SceneFlowController {get;}
    public AppDependencies(ISceneFlowController sceneFlowController)
    {
        SceneFlowController = sceneFlowController ?? throw new ArgumentNullException(nameof(SceneFlowController));
        
    }
}
