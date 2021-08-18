using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemGameLose : BaseItem<Player>
{
    public override bool IsUseful => false;
    public override void OnTargetEnter(Player target)
    {
        GameManager.Ins.GameLose();
    }

    public override void OnTargetExit(Player target)
    {

    }
}
