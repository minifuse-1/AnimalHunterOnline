using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

[CreateAssetMenu(fileName = "PlayerMotorSettingsSO", menuName = "Custom/PlayerMotorSettingsSO")]
public sealed class PlayerMotorSettingsSO : ScriptableObject
{
    [SerializeField][Range(0f, 10f)] private float _movementSpeed;
    [SerializeField][Range(0f, 10f)] private float _jumpForce;
    [SerializeField][Range(0, 10)] private uint _maxJumps;
    [SerializeField] private LayerMask _jumpResetLayer;

    public float MovementSpeed => _movementSpeed;
    public float JumpForce => _jumpForce;
    public uint MaxJumps => _maxJumps;
    public LayerMask JumpResetLayer => _jumpResetLayer;

#if UNITY_EDITOR
    private void OnValidate()
    {
        if (_movementSpeed == 0 || _jumpForce == 0 || _maxJumps == 0) Debug.LogWarning("movement speed, jump force, max jumps is unset!");
    }
#endif
}
