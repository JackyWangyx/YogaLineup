using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public abstract class StoreSetting<TSetting, TStoreData> : SettingBase<TSetting>
    where TSetting : StoreSetting<TSetting, TStoreData>
    where TStoreData : StoreData
{
    [TableList] public List<TStoreData> Datas;
    public List<int> Costs;

    public override void Init()
    {
        base.Init();
        foreach (var data in Datas)
        {
            data.Init();
        }
    }
}
