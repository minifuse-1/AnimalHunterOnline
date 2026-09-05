using UnityEngine;

public interface IPlayerInputSource
{
    public void Enable();
    public void Disable();
    public PlayerInputFrame GetPlayerInputFrame();
}