using UnityEngine;

public readonly struct PlayerCommand
{
    public uint Sequence { get; } // Could be problem if game session is longer than 20 min
    public Vector2 MoveDir { get; }
    public PlayerLookAngles Look { get; }
    public bool JumpPressed { get; }

    public PlayerCommand(uint sequence, Vector2 moveDir,  PlayerLookAngles look, bool jumpPressed)
    {
        Sequence = sequence; 
        MoveDir = moveDir; 
        Look = look; 
        JumpPressed = jumpPressed;
    }
}
