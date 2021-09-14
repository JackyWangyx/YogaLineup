using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemGameLose : ItemBase<Player>
{
    public override bool IsUseful => false;
    public override void OnTargetEnter(Player target)
    {
        Game.Enter<GameLose>();
    }

    public override void OnTargetExit(Player target)
    {

    }
}
