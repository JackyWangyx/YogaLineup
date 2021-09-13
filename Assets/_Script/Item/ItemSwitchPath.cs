using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSwitchPath : BaseItem<Player>
{
    public LevelPath Path;

    public override void OnTargetEnter(Player target)
    {
        var lastPath = Level.CurrentPath;

        Level.CurrentPath = Path;
        Level.CurrentPath.Move(0f);
    }

    public override void OnTargetExit(Player target)
    {
       
    }
}
