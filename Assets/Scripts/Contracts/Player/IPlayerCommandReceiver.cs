using UnityEngine;

public interface IPlayerCommandReceiver
{
    PlayerId PlayerId { get; }
    public void Receive(in PlayerCommand playerCommand);
}
