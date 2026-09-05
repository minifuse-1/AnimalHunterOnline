using System;
using UnityEngine;

public sealed class RigidBodyPlayerMotor : MonoBehaviour, IPlayerMotor
{
    [SerializeField] private Rigidbody _rb;
    [SerializeField] private PlayerMotorSettingsSO _settingsSO;
    private Vector2 _moveIntent;
    private float _bodyYawDegree;
    private uint _remainingJumps;
    private bool _jumpQueued;

    private void Awake()
    {
        ResetJumps();
    }

    private void FixedUpdate()
    {
        HandleMove();
        HandleJumping();
    }

    private void HandleJumping()
    {
        if (!_jumpQueued || _remainingJumps <= 0) return;

        _jumpQueued = false;
        _remainingJumps--;
        _rb.linearVelocity = new Vector3(_rb.linearVelocity.x, 0f, _rb.linearVelocity.z);
        _rb.AddForce(_settingsSO.JumpForce * Vector3.up, ForceMode.Impulse);
    }

    private void HandleMove()
    {
        _rb.angularVelocity = Vector3.zero;
        Vector3 worldMovement = Vector3.ClampMagnitude(transform.right * _moveIntent.x + transform.forward * _moveIntent.y, 1f);
        _rb.linearVelocity = new Vector3(worldMovement.x * _settingsSO.MovementSpeed, _rb.linearVelocity.y, worldMovement.z * _settingsSO.MovementSpeed);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!HitJumpResetLayer(other)) return;

        ResetJumps();
    }

    private bool HitJumpResetLayer(Collider other)
    {
        return (_settingsSO.JumpResetLayer.value & (1 << other.gameObject.layer)) != 0; // Bit magic
    }

    public void RequestJump()
    {
        if (_remainingJumps <= 0) return;

        _jumpQueued = true;
    }

    public void SetMovementIntent(Vector2 moveInput, float bodyYawDegree)
    {
        _moveIntent = Vector2.ClampMagnitude(moveInput, 1f);
        _bodyYawDegree = bodyYawDegree;
    }

    private void ResetJumps()
    {
        _remainingJumps = _settingsSO.MaxJumps;
    }
}
