using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemGameLose : ItemBase<Player>
{
    public override bool IsUseful => false;
    public override void OnTargetEffect(Player target)
    {
        if (!target.IsPlayer) return;
        Game.Enter<GameLose>();
    }
}
