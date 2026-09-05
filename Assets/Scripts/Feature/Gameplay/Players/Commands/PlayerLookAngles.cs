using UnityEngine;

public readonly struct PlayerLookAngles
{
    public float YawDegree { get; }
    public float PitchDegree { get; }

    public PlayerLookAngles(float yawDegree, float pitchDegree)
    {
        YawDegree = yawDegree;
        PitchDegree = pitchDegree;
    }
}
