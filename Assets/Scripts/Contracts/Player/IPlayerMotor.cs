using UnityEngine;

public interface IPlayerMotor
{
    public void SetMovementIntent(Vector2 moveInput, float bodyYawDegree);
    public void RequestJump();
}
