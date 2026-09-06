using System;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public class LocalMatchCompletionCoordinator : IDisposable, IPostMatchFlowSource
{
    public event Action<PostMatchFlowInfo> PostMatchStarted;
    private readonly IMatchResultSource _matchResultSource;
    private readonly IMatchResultSink _matchResultSink;
    private readonly IPlayerInputSource _playerInputSource;
    private readonly ISceneFlowController _sceneFlowController;
    private readonly ScenePathSO _returnScenePathSO;
    private readonly TimeSpan _minReturnDelay;
    private readonly CancellationTokenSource _lifetimeCancellation;
    private bool _isHandlingCompletion;

    public LocalMatchCompletionCoordinator
        (
        IMatchResultSource matchResultSource, 
        IMatchResultSink matchResultSink, 
        IPlayerInputSource playerInputSource, 
        ISceneFlowController sceneFlowController, 
        ScenePathSO returnScenePathSO, 
        TimeSpan minReturnDelay, 
        CancellationToken ct
        )
    {
        _matchResultSource = matchResultSource ?? throw new ArgumentNullException(nameof(matchResultSource));
        _matchResultSink = matchResultSink ?? throw new ArgumentNullException(nameof(matchResultSink));
        _playerInputSource = playerInputSource ?? throw new ArgumentNullException(nameof(playerInputSource));
        _sceneFlowController = sceneFlowController ?? throw new ArgumentNullException(nameof(sceneFlowController));
        _returnScenePathSO = returnScenePathSO != null ? returnScenePathSO : throw new ArgumentNullException(nameof(returnScenePathSO));
        _minReturnDelay = minReturnDelay;
        _lifetimeCancellation = CancellationTokenSource.CreateLinkedTokenSource(ct);
        _matchResultSource.MatchCompleted += CompleteMatch;
    }

    public void Dispose()
    {
        _matchResultSource.MatchCompleted -= CompleteMatch;
        _lifetimeCancellation.Cancel();
        _lifetimeCancellation.Dispose();
    }

    private async void CompleteMatch(MatchResult matchResult)
    {
        if (_isHandlingCompletion) return;
        _isHandlingCompletion = true;

        PostMatchStarted?.Invoke(new PostMatchFlowInfo(_minReturnDelay));
        await HandleCompletionAsync(matchResult, _lifetimeCancellation.Token);
    }

    private async Task HandleCompletionAsync(MatchResult matchResult, CancellationToken ct)
    {
        Task delayTask = Task.Delay(_minReturnDelay, ct);

        try
        {
            await _matchResultSink.HandleResultAsync(matchResult, ct);
        }
        catch (OperationCanceledException)
        {
            return;
        }

        catch (Exception ex) {
            Debug.LogException(ex);
        }

        try
        {
            await delayTask;
            ct.ThrowIfCancellationRequested();
        }
        catch (OperationCanceledException)
        {
            return;
        }
        catch (Exception ex)
        {
            Debug.LogException(ex);
        }

        _playerInputSource.Disable();
        _sceneFlowController.ChangePrimaryScene(_returnScenePathSO);
    }
}
