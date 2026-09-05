using UnityEngine;

public class PlayerActor : MonoBehaviour, IPlayerActor
{
    [SerializeField] private RigidBodyPlayerMotor _rigidBodyPlayerMotor;
    [SerializeField] private PlayerLookController _playerlookController;
    [SerializeField] private PlayerProfilePresenter _presenter;

    private IPlayerMotor _playerMotor;
    private IPlayerLookController _lookController;

    public GameObject RootObject => gameObject;

    public PlayerId PlayerId { get; private set; }

    public void Initialize(PlayerProfile playerProfile)
    {
        PlayerId = playerProfile.PlayerId;
        _presenter.Apply(playerProfile);
        _playerMotor = _rigidBodyPlayerMotor;
        _lookController = _playerlookController;
    }

    public void Receive(in PlayerCommand playerCommand)
    {
        _lookController.SetLookDirection(playerCommand.Look);
        _playerMotor.SetMovementIntent(playerCommand.MoveDir, playerCommand.Look.YawDegree);

        if (playerCommand.JumpPressed) _playerMotor.RequestJump();
    }
}
