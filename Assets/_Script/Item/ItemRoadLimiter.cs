using UnityEngine;

public class ItemRoadLimiter : ItemBase<Player>
{
    [Header("Limit")]
    public Vector2 Width;

    public override void OnTargetEnter(Player target)
    {
        target.State.TurnRange = Width;
    }

    public override void OnTargetExit(Player target)
    {

    }
}