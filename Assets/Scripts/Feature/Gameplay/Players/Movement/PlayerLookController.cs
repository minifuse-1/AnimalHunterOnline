using UnityEngine;

public class PlayerLookController : MonoBehaviour, IPlayerLookController
{
    [SerializeField] private Transform _bodyRoot;
    [SerializeField] private Transform _viewRoot;

    public void SetLookDirection(PlayerLookAngles playerLookAngles)
    {
        float pitch = Mathf.Clamp(playerLookAngles.PitchDegree, -89f, 89f);
        _bodyRoot.localRotation = Quaternion.Euler(0f, playerLookAngles.YawDegree, 0f);
        _viewRoot.localRotation = Quaternion.Euler(-pitch, 0f, 0f);
    }
}
