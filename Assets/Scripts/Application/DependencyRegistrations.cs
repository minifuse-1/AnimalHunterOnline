
public static class DependencyRegistrations
{
    public static GameAppBuilder Add(this GameAppBuilder builder, IMatchResultSink matchResultSink)
    {
        builder.RegisterMatchResultSink(matchResultSink);
        return builder;
    }
    public static GameAppBuilder Add(this GameAppBuilder builder, ISceneLoader sceneLoader)
    {
        builder.RegisterSceneLoader(sceneLoader);
        return builder;
    }

    public static GameAppBuilder Add(
        this GameAppBuilder builder,
        SceneFlowController sceneFlowController
    )
    {
        builder.RegisterSceneFlowController(sceneFlowController);
        return builder;
    }

    public static GameAppBuilder Add(this GameAppBuilder builder, LoadingScreenController loadingScreenController)
    {
        builder.RegisterLoadingScreenController(loadingScreenController);
        return builder;
    }

    public static GameAppBuilder Add(this GameAppBuilder builder, StartupScenes scenes)
    {
        builder.RegisterStartupScenes(scenes);
        return builder;
    }

    public static GameAppBuilder Add(this GameAppBuilder builder, IAuthenticationClient authenticationClient)
    {
        builder.RegisterAuthenticationClient(authenticationClient);
        return builder;
    }
    public static GameAppBuilder Add(this GameAppBuilder builder, IPlayerProfileClient playerProfileClient)
    {
        builder.RegisterPlayerProfileClient(playerProfileClient);
        return builder;
    }
    public static GameAppBuilder Add(this GameAppBuilder builder, IAuthenticationSessionStore authenticationSessionStore)
    {
        builder.RegisterAuthenticationSessionStore(authenticationSessionStore);
        return builder;
    }
}
