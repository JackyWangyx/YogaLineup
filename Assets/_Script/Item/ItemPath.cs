using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class ItemPath : ItemBase<Player>
{
    [BoxGroup("Path")] public LevelBlock Block;
    [BoxGroup("Path")] public bool SwitchPath;
    [BoxGroup("Path"), ShowIf("SwitchPath")] public int SwitchPathIndex;

    [BoxGroup("Path")] public bool LimitRange;
    [BoxGroup("Path"), ShowIf("LimitRange")] public Vector2 Range;

    protected override void Awake()
    {
        base.Awake();
        Block = GetComponentInParent<LevelBlock>();
    }

    public override void OnTargetEnter(Player target)
    {
        if (SwitchPath)
        {
            Block.PathIndex = SwitchPathIndex;
            // CurrentLevel.CurrentPath.Move(0f);
        } 

        if (LimitRange)
        {
            Player.TurnRange = Range;
        }
    }

    public override void OnTargetExit(Player target)
    {

    }
}
