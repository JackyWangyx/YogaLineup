using UnityEngine;

public class ItemRoadLimiter : BaseItem<Player>
{
    [Header("Limit")]
    public Vector2 Range;
    public bool RestoreLevelRange;

    public override void OnTargetEnter(Player target)
    {
        if (RestoreLevelRange)
        {
            target.TurnRange = GameManager.Ins.Level.TurnRange;
        }
        else
        {
            target.TurnRange = Range;
        }
    }

    public override void OnTargetExit(Player target)
    {

    }
}