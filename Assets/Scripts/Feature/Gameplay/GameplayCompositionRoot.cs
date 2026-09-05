using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameplayCompositionRoot : MonoBehaviour, ILoadedSceneCompositionRoot
{
    [SerializeField] private PlayerActor _playerPrefab;
    [SerializeField] private LocalPlayerInputSource _playerInputSource;
    [SerializeField] private LocalPlayerCommandProducer _playerCommandProducer;
    [SerializeField] private List<Transform> _spawnPoints;
    [SerializeField] private ScenePathSO _gameplayUISceneSO;

    private void OnDestroy()
    {
        _playerInputSource?.Disable();
    }
    public void Initialize(AppDependencies dependencies)
    {
        if (!dependencies.PlayerProfileContext.TryGetCurrentProfile(out PlayerProfile profile)) throw new InvalidOperationException(nameof(Initialize));
        
        List<ISceneNavActions> navActions = GetComponentsInChildren<ISceneNavActions>().ToList();
        navActions.ForEach(action => action.Bind(dependencies.SceneFlowController));

        IPlayerRegistry playerRegistry = new PlayerRegistry();
        IPlayerSpawner playerSpawner = new PlayerSpawner(_playerPrefab, playerRegistry);
        SpawnPointSet spawnPointSet = new SpawnPointSet(_spawnPoints);
        IPlayerLifecycleCoordinator playerLifecycleCoordinator = new PlayerLifecycleCoordinator(playerSpawner, spawnPointSet);
        IPlayerActor player = playerLifecycleCoordinator.AddPlayer(profile);

        IPlayerCommandDispatcher commandDispatcher = new PlayerCommandDispatcher(playerRegistry);
        IPlayerCommandSink playerCommandSink = new BoundLocalPlayerCommandSink(commandDispatcher, player.PlayerId);
        _playerCommandProducer.Bind(_playerInputSource, playerCommandSink);

        _playerInputSource.Enable();

        dependencies.SceneFlowController.AddScene(_gameplayUISceneSO);
    }
}
