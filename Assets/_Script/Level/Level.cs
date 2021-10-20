using System;
using System.Collections.Generic;
using UnityEngine;
using Aya.Extension;
using Sirenix.OdinInspector;

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
        SpawnBlocks();
        ItemList = transform.GetComponentsInChildren<ItemBase>().ToList();
        foreach (var item in ItemList)
        {
            item.Init();
        }

        Player.Init();
    }

    public void InitItemsRenderer()
    {
        ItemList.ForEach(item => item.InitRenderer());
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

    [ButtonGroup("Test"), Button("Test Create")]
    public void SpawnBlocks()
    {
        DeSpawnLevelBlocks();

        var currentPos = Vector3.zero;
        var currentForward = Vector3.forward;
        foreach (var levelBlockPrefab in BlockList)
        {
            var blockIns = Application.isPlaying ? GamePool.Spawn(levelBlockPrefab, transform) : Instantiate(levelBlockPrefab, transform);
            blockIns.transform.position = currentPos;
            blockIns.transform.forward = currentForward;
            blockIns.Init();

            currentPos = blockIns.EndPosition;
            currentForward = blockIns.EndForward;

            BlockInsList.Add(blockIns);
        }
    }

    [ButtonGroup("Test"), Button("Destroy"), GUIColor(1f, 0.5f, 0.5f)]
    public void DeSpawnLevelBlocks()
    {
        if (!Application.isPlaying)
        {
            BlockInsList = GetComponentsInChildren<LevelBlock>().ToList();
        }

        foreach (var levelBlock in BlockInsList)
        {
            if (Application.isPlaying)
            {
                GamePool.DeSpawn(levelBlock);
            }
            else
            {
                DestroyImmediate(levelBlock.gameObject);
            }
        }

        BlockInsList.Clear();
    }
}
