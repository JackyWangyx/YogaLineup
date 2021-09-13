using UnityEngine;

public class ItemRoadLimiter : BaseItem<Player>
{
    [Header("Limit")]
    public Vector2 Width;

    public override void OnTargetEnter(Player target)
    {
        target.TurnRange = Width;
    }

    public override void OnTargetExit(Player target)
    {

    }
}