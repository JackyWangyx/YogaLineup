using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class ItemPath : ItemBase<Player>
{
    [BoxGroup("Path")] public bool SwitchPath;
    [BoxGroup("Path"), ShowIf("SwitchPath")] public int SwitchPathIndex;

    [BoxGroup("Path")] public bool LimitRange;
    [BoxGroup("Path"), ShowIf("LimitRange")] public Vector2 Range;

    [BoxGroup("Path")] public bool KeepDirection;

    public LevelBlock Block { get; set; }

    protected override void Awake()
    {
        base.Awake();
        Block = GetComponentInParent<LevelBlock>();
    }

    public override void OnTargetEnter(Player target)
    {
        if (SwitchPath)
        {
            target.PathFollower.SwitchPath(SwitchPathIndex);
        } 

        if (LimitRange)
        {
            target.State.TurnRange = Range;
        }
        else
        {
            target.State.TurnRange = target.PathFollower.CurrentPath.TurnRange;
        }

        target.State.KeepDirection = KeepDirection;
    }

    public override void OnTargetExit(Player target)
    {

    }
}
