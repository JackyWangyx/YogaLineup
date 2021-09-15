using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSwitchPath : ItemBase<Player>
{
    public LevelPath Path;

    public override void OnTargetEnter(Player target)
    {
        var lastPath = CurrentLevel.CurrentPath;

        CurrentLevel.CurrentPath = Path;
        CurrentLevel.CurrentPath.Move(0f);
    }

    public override void OnTargetExit(Player target)
    {
       
    }
}
