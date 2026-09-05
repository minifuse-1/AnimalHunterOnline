using UnityEngine;

public interface IPlayerCommandSink
{
    public void Submit(in PlayerCommand playerCommand);
}
