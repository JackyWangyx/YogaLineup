using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemGameWin : BaseItem<Player>
{
    public override bool IsUseful => false;
    public override void OnTargetEnter(Player target)
    {
        GameManager.Ins.GameWin();
    }

    public override void OnTargetExit(Player target)
    {

    }
}
