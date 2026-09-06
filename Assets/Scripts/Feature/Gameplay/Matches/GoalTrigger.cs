using System;
using UnityEngine;

public sealed class GoalTrigger : MonoBehaviour
{
    [SerializeField] private LayerMask _playerLayer;

    private IMatchController _matchController;

    private void OnTriggerEnter(Collider other)
    {
        if ((_playerLayer.value & (1 << other.gameObject.layer)) == 0) return;

        Debug.Log("Player hit the goal!");

        PlayerActor playerActor = other.GetComponentInParent<PlayerActor>();

        if (playerActor == null) return;

        _matchController.TryDeclareWinner(playerActor.PlayerId);
    }

    public void Bind(IMatchController matchController)
    {
        _matchController = matchController ?? throw new ArgumentNullException(nameof(matchController));
    }
}
