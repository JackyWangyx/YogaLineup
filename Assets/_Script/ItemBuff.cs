using System.Reflection;
using UnityEngine;

public enum BuffType
{
    None = -1,
    Invincible = 1,
    Shift = 2,
}

public class ItemBuff : ItemBase<Player>
{
    [Header("Buff")]
    public BuffType Type = BuffType.None;
    public float Duration = 1f;
    public float Arg1;
    public float Arg2;
    public float Arg3;
    public float Arg4;

    public override void OnTargetEnter(Player target)
    {
        var buffType = Assembly.GetExecutingAssembly().GetType("Buff" + Type);
        target.Buff.AddBuff(buffType, Duration, new[] {Arg1, Arg2, Arg3, Arg4});
    }

    public override void OnTargetExit(Player target)
    {
        
    }
}