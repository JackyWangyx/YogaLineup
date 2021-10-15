using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class ItemCoin : ItemBase<Player>
{
    [BoxGroup("Coin")] public int Value = 1;

    public override void OnTargetEnter(Player target)
    {
        Save.Coin.Value += Value;
        if (Save.Coin < 0) Save.Coin.Value = 0;
    }

    public override void OnTargetExit(Player target)
    {
        
    }
}
