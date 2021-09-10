using UnityEngine;

public class ItemRoadLimiter : BaseItem<Player>
{
    [Header("Limit")]
    public Vector2 Range;

    public override void OnTargetEnter(Player target)
    {
        target.TurnRange = Range;
    }

    public override void OnTargetExit(Player target)
    {

    }
}