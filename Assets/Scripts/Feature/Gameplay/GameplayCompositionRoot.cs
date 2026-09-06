using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameplayCompositionRoot : MonoBehaviour, ILoadedSceneCompositionRoot
{
    [SerializeField] private PlayerActor _playerPrefab;
    [SerializeField] private GoalTrigger _goalTrigger;
    [SerializeField] private LocalPlayerInputSource _playerInputSource;
    [SerializeField] private LocalPlayerCommandProducer _playerCommandProducer;
    [SerializeField] private List<Transform> _spawnPoints;
    [SerializeField] private ScenePathSO _gameplayUISceneSO;
    [SerializeField] private ScenePathSO _postMatchSceneSO;

    private IGameplaySessionService _gameplaySessionService;
    private IDisposable _matchCompletionCoordinator;

    private void OnDestroy()
    {
        _playerInputSource?.Disable();
        _gameplaySessionService?.ClearCurrentSession();
        _matchCompletionCoordinator?.Dispose();
    }
    public void Initialize(AppDependencies dependencies)
    {
        if (!dependencies.PlayerProfileContext.TryGetCurrentProfile(out PlayerProfile profile)) throw new InvalidOperationException(nameof(Initialize));
        
        List<ISceneNavActions> navActions = GetComponentsInChildren<ISceneNavActions>().ToList();
        navActions.ForEach(action => action.Bind(dependencies.SceneFlowController));

        // match handling
        MatchId matchId = new(Guid.NewGuid());
        IMatchController matchController = new MatchController(matchId);
        LocalMatchCompletionCoordinator completionCoordinator = new LocalMatchCompletionCoordinator(
            matchController,
            dependencies.MatchResultSink,
            _playerInputSource,
            dependencies.SceneFlowController,
            _postMatchSceneSO,
            TimeSpan.FromSeconds(5),
            destroyCancellationToken
            );
        _matchCompletionCoordinator = completionCoordinator; 
        LocalGameplaySessionContext gameplaySessionContext = new(completionCoordinator, matchController);
        dependencies.GameplaySessionService.SetCurrentSession(gameplaySessionContext);
        _gameplaySessionService = dependencies.GameplaySessionService;
        _goalTrigger.Bind(matchController);

        // Player lifecylce
        IPlayerRegistry playerRegistry = new PlayerRegistry();
        IPlayerSpawner playerSpawner = new PlayerSpawner(_playerPrefab, playerRegistry);
        SpawnPointSet spawnPointSet = new SpawnPointSet(_spawnPoints);
        IPlayerLifecycleCoordinator playerLifecycleCoordinator = new PlayerLifecycleCoordinator(playerSpawner, spawnPointSet);

        // Add each player
        IPlayerActor player = playerLifecycleCoordinator.AddPlayer(profile);

        // Commands
        IPlayerCommandDispatcher commandDispatcher = new PlayerCommandDispatcher(playerRegistry);
        IPlayerCommandSink playerCommandSink = new BoundLocalPlayerCommandSink(commandDispatcher, player.PlayerId);
        _playerCommandProducer.Bind(_playerInputSource, playerCommandSink);

        _playerInputSource.Enable();

        dependencies.SceneFlowController.AddScene(_gameplayUISceneSO);
    }
}
