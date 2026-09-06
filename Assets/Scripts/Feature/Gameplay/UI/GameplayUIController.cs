using System;
using TMPro;
using UnityEngine;

public sealed class GameplayUIController : MonoBehaviour
{
    [SerializeField] private GameObject _postGamePanel;
    [SerializeField] private TMP_Text _winnerText;
    [SerializeField] private TMP_Text _timerText;
    [SerializeField] private TMP_Text _displayNameText;
    [SerializeField] private TMP_Text _playerIdText;

    private IPlayerProfileContext _playerProfileContext;
    private IMatchResultSource _matchResultSource;
    private IPostMatchFlowContext _postMatchFlowContext;
    private float _timer;

    private void Update()
    {
        if (_timer <= 0f) return;
        _timer = Mathf.Max(0f, _timer - Time.unscaledDeltaTime);

        _timerText.text = Mathf.CeilToInt(_timer).ToString();

    }

    private void OnDestroy()
    {
        UnBind();
    }

    public void Bind(IPlayerProfileContext playerProfileContext, IMatchResultSource matchResultSource, IPostMatchFlowContext postMatchFlowContext)
    {
        UnBind();
        _playerProfileContext = playerProfileContext ?? throw new ArgumentNullException(nameof(playerProfileContext));
        _matchResultSource = matchResultSource ?? throw new ArgumentNullException(nameof(matchResultSource));
        _postMatchFlowContext = postMatchFlowContext ?? throw new ArgumentNullException(nameof(postMatchFlowContext));
        _matchResultSource.MatchCompleted += SetWinnerText;
        _postMatchFlowContext.PostMatchFlowSource.PostMatchStarted += HandlePostMatch;
        PlayerProfile profile = _playerProfileContext.CurrentProfile;
        _displayNameText.text = profile.DisplayName;
        _playerIdText.text = profile.PlayerId.ToString();
        _postGamePanel.SetActive(false);
    }

    private void UnBind()
    {
        if (_matchResultSource != null)
            _matchResultSource.MatchCompleted -= SetWinnerText;

        if (_postMatchFlowContext != null)
            _postMatchFlowContext.PostMatchFlowSource.PostMatchStarted -= HandlePostMatch;

        _matchResultSource = null;
        _postMatchFlowContext = null;
        _playerProfileContext = null;

        _postGamePanel.SetActive(false);
        _winnerText.text = string.Empty;
        _timerText.text = string.Empty;
        _displayNameText.text = string.Empty;
        _playerIdText.text = string.Empty;
    }

    private void SetWinnerText(MatchResult matchResult)
    {
        _winnerText.text = matchResult.WinnerId.ToString();
    }

    private void HandlePostMatch(PostMatchFlowInfo info)
    {
        _postGamePanel.SetActive(true);
        _timer = (float)info.MinPostMatchDuration.TotalSeconds;
    }
}
