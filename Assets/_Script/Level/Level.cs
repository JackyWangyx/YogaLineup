using System.Collections.Generic;
using UnityEngine;
using Aya.Extension;

public class Level : GameEntity
{ 
    public List<LevelBlock> BlockList;
    public List<BaseItem> ItemList { get; set; }

    public List<LevelBlock> BlockInsList { get; set; } = new List<LevelBlock>();


    protected override void Awake()
    {
        base.Awake();
        ItemList = transform.GetComponentsInChildren<BaseItem>().ToList();
    }

    public void Init()
    {
        foreach (var item in ItemList)
        {
            item.Init();
        }

        Finish = false;
        SpawnBlocks();
        EnterBlock(0);
        Player.Init();
    }

    public void SpawnBlocks()
    {
        foreach (var levelBlock in BlockInsList)
        {
            GamePool.DeSpawn(levelBlock);
        }

        BlockInsList.Clear();

        var currentPos = Vector3.zero;
        var currentForward = Vector3.forward;
        foreach (var levelBlock in BlockList)
        {
            var blockIns = GamePool.Spawn(levelBlock, transform);
            blockIns.transform.position = currentPos;
            blockIns.transform.forward = currentForward;
            blockIns.Init();

            currentPos = blockIns.EndPosition;
            currentForward = blockIns.EndForward;

            BlockInsList.Add(blockIns);
        }
    }

    public int CurrentBlockIndex { get; set; }
    public LevelBlock CurrentBlock { get; set; }
    public LevelPath CurrentPath => CurrentBlock.Path;
    public bool Finish { get; set; }
    
    public bool EnterBlock(int index, float initDistance = 0f)
    {
        if (index >= BlockInsList.Count)
        {
            Finish = true;
            return false;
        }

        CurrentBlockIndex = index;
        CurrentBlock = BlockInsList[index];
        CurrentPath.Enter(initDistance);
        return true;
    }

    public Vector3 Move(float distance)
    {
        Vector3 result;

        while (true)
        {
            var overDistance = 0f;
            var finish = false;
            (finish, result, overDistance) = CurrentPath.Move(distance);
            if (finish)
            {
                var enterResult = EnterBlock(CurrentBlockIndex + 1, overDistance);
                if (!enterResult) break;
                distance = 0f;
            }
            else
            {
                break;
            }
        }

        return result;
    }
}
