using System;
using System.Collections.Generic;
using UnityEngine;
using Aya.Extension;

public class Level : GameEntity
{ 
    public List<LevelBlock> BlockList;

    public List<ItemBase> ItemList { get; set; }
    public Dictionary<Type, List<ItemBase>> ItemDic { get; set; }

    public List<LevelBlock> BlockInsList { get; set; } = new List<LevelBlock>();

    protected override void Awake()
    {
        base.Awake();
    }

    public void Init()
    {
        Finish = false;
        SpawnBlocks();
        ItemList = transform.GetComponentsInChildren<ItemBase>().ToList();
        foreach (var item in ItemList)
        {
            item.Init();
        }

        EnterBlock(0);
        Player.Init();
        Player.transform.position = Move(0f);
    }

    public List<T> GetItems<T>() where T : ItemBase
    {
        if (!ItemDic.TryGetValue(typeof(T), out var list))
        {
            list = new List<ItemBase>();
            ItemDic.Add(typeof(T), list);
        }

        return list as List<T>;
    }

    public T GetItem<T>() where T : ItemBase
    {
        var list = GetItems<T>();
        return list.First();
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
    public new LevelBlock CurrentBlock { get; set; }

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
        Player.State.TurnRange = CurrentBlock.TurnRange;

        return true;
    }

    public Vector3 Move(float distance)
    {
        if (Finish) return CurrentPath.GetPosition(1f);
        Vector3 result;

        while (true)
        {
            var overDistance = 0f;
            var finish = false;
            (finish, result, overDistance) = CurrentPath.Move(distance);
            if (finish)
            {
                var enterResult = EnterBlock(CurrentBlockIndex + 1, overDistance);
                if (!enterResult)
                {
                    Finish = true;
                    break;
                }
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
