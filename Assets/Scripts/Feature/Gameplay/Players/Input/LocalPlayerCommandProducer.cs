using UnityEngine;
using System;

public sealed class LocalPlayerCommandProducer : MonoBehaviour
{
    private IPlayerInputSource _playerInputSource;
    private IPlayerCommandSink _playerCommandSink;

    private uint _sequence;
    private float _yaw;
    private float _pitch;
    private bool _isReady;

    private void Update()
    {
        if (!_isReady) return;
        PlayerInputFrame frame = _playerInputSource.GetPlayerInputFrame();
        PlayerCommand command = CreateNewCommand(frame);
        _playerCommandSink.Submit(command);
    }

    private PlayerCommand CreateNewCommand(PlayerInputFrame frame)
    {
        _yaw = Mathf.Repeat(_yaw + frame.LookDelta.x, 360f);
        _pitch = Mathf.Clamp(_pitch + frame.LookDelta.y, -89f, 89f);
        _sequence++;
        PlayerLookAngles look = new(_yaw, _pitch);
        return new PlayerCommand(_sequence, frame.MoveDir, look, frame.JumpPressed);
    }

    public void Bind(IPlayerInputSource playerInputSource, IPlayerCommandSink playerCommandSink)
    {
        _playerInputSource = playerInputSource ?? throw new ArgumentNullException(nameof(playerInputSource));
        _playerCommandSink = playerCommandSink ?? throw new ArgumentNullException(nameof(playerCommandSink));
        _isReady = true;
    }


}
