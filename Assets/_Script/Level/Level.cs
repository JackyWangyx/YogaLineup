using System;
using System.Collections.Generic;
using UnityEngine;
using Aya.Extension;
using Sirenix.OdinInspector;

public class Level : GameEntity
{
    public int PlayerCount = 1;
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
        InitBlocks();
        InitItem();
        InitPlayer();
    }

    #region Item
    
    public void InitItem()
    {
        ItemList = transform.GetComponentsInChildren<ItemBase>(true).ToList();
        foreach (var item in ItemList)
        {
            item.Init();
        }

        ItemList = transform.GetComponentsInChildren<ItemBase>(true).ToList();

        ItemDic = new Dictionary<Type, List<ItemBase>>();
        foreach (var item in ItemList)
        {
            var itemType = item.GetType();
            if (!ItemDic.TryGetValue(itemType, out var itemList))
            {
                itemList = new List<ItemBase>();
                ItemDic.Add(itemType, itemList);
            }

            itemList.Add(item);
        }
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

        return list.ToList(i => i as T);
    }

    public T GetItem<T>() where T : ItemBase
    {
        var list = GetItems<T>();
        return list.First();
    }

    public void RemoveItem(ItemBase item)
    {
        var type = item.GetType();
        ItemList.Remove(item);
        if (ItemDic.TryGetValue(type, out var list))
        {
            list.Remove(item);
        }
    } 

    #endregion

    public void InitPlayer()
    {
        Game.PlayerList.Clear();
        for (var i = 0; i < PlayerCount; i++)
        {
            var playerIns = GamePool.Spawn(Game.PlayerPrefab, null);
            playerIns.State.Index = i;
            playerIns.State.IsPlayer = false;
            Game.PlayerList.Add(playerIns);
        }

        var player = Game.PlayerList.Random();
        player.State.IsPlayer = true;
        Game.Player = player;

        foreach (var playerTemp in Game.PlayerList)
        {
            playerTemp.Init();
        }
    }

    [ButtonGroup("Test"), Button("Test Create")]
    public void InitBlocks()
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
