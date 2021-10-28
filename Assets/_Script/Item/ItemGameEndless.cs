using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemGameEndless : ItemBase<Player>
{
    public override bool IsUseful => false;
    public override void OnTargetEnter(Player target)
    {
        if (!target.IsPlayer) return;
        Game.Enter<GameEndless>();
    }

    public override void OnTargetExit(Player target)
    {

    }
}
