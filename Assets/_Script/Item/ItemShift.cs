using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemShift : ItemBase<Player>
{
    public float SpeedChange = 1f;
    public float Duration = 1f;

    public override void OnTargetEffect(Player target)
    {
        target.Buff.AddBuff<BuffShift>(Duration, new[] { SpeedChange });
    }
}
