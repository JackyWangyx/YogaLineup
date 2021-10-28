using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class ItemCoin : ItemBase<Player>
{
    [BoxGroup("Coin")] public int Value = 1;
    [BoxGroup("Coin")] public GameObject CoinPrefab;
    [BoxGroup("Coin")] public int FlyIconCount;

    public override void OnTargetEnter(Player target)
    {
        if (!target.IsPlayer) return;
        UIGame.Ins.FlyCoin(CoinPrefab, FlyIconCount, () =>
        {
            Save.Coin.Value += Value;
        });
    }

    public override void OnTargetExit(Player target)
    {
        
    }
}
