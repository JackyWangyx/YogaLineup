using System;
using System.Collections;
using System.Collections.Generic;
using Aya.Extension;
using Sirenix.OdinInspector;
using UnityEngine;

[Serializable]
public class ItemGroupData : StoreData
{
    public new ItemGroupSetting Setting => base.Setting as ItemGroupSetting;

    [TableColumnWidth(100)]
    public List<GameObject> Prefabs;

    public GameObject this[int index]
    {
        get
        {
            if (Prefabs.Count == 0) return default;
            if (Prefabs.Count < index) return Prefabs.Last();
            return Prefabs[index];
        } 
    }
}
