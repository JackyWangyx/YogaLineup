using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class ItemKey : ItemBase<Player>
{
    [BoxGroup("Key")] public int Value = 1;

    public override void OnTargetEnter(Player target)
    {
        if (!target.IsPlayer) return;
        UIFlyCoin.Ins.Fly(UIFlyCoin.Key, UIGame.Ins.FlyCoinStart.position, Value, () =>
        {
            Save.Key.Value += 1;
        });
    }

    public override void OnTargetExit(Player target)
    {
        
    }
}
