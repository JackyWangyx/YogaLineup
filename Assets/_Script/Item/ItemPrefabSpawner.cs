using System.Collections;
using System.Collections.Generic;
using Aya.Extension;
using UnityEngine;

public class ItemPrefabSpawner : ItemBase<Player>
{
    public override void Init()
    {
        base.Init();

        var itemList = new List<ItemBase>();
        foreach (var instance in RenderInstanceList)
        {
            var items = instance.GetComponentsInChildren<ItemBase>();
            itemList.AddRange(items);
        }
       
        itemList.ForEach(i => i.Init());
    }

    public override void InitRenderer()
    {
        base.InitRenderer();

        var itemList = new List<ItemBase>();
        foreach (var instance in RenderInstanceList)
        {
            var items = instance.GetComponentsInChildren<ItemBase>();
            itemList.AddRange(items);
        }

        itemList.ForEach(i => i.InitRenderer());
    }

    public override void OnTargetEffect(Player target)
    {
        
    }
}
