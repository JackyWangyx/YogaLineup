using System;
using System.Collections.Generic;
using Aya.Data.Persistent;
using Aya.Extension;
using Sirenix.OdinInspector;
using UnityEngine;

public enum UnlockMode
{
    FixedValue = 0,
    ChangeValue = 1,
}

public abstract class StoreSetting<TSetting, TStoreData> : SettingBase<TSetting>, IStoreSetting<TStoreData>
    where TSetting : StoreSetting<TSetting, TStoreData>
    where TStoreData : StoreData
{
    [TableList] public List<TStoreData> Datas;
    public List<int> Costs;
    public List<int> UnlockValues;

    public virtual string SaveKey => typeof(TStoreData).Name.Replace("Data", "");

    public TStoreData CurrentSelectData
    {
        get
        {
            var index = SelectIndex;
            if (index < 0) return default;
            return Datas[index];
        }
    }

    public TStoreData CurrentUnlockData
    {
        get
        {
            var index = UnlockIndex;
            if (index < 0) return default;
            return Datas[index];
        }
    }

    [NonSerialized] public sInt SaveSelectIndex;
    [NonSerialized] public sFloat SaveUnlockProgress;
    [NonSerialized] public sInt SaveUnlockIndex;
    [NonSerialized] public sInt SaveUnlockCount;

    public int SelectIndex
    {
        get => SaveSelectIndex.Value;
        set => SaveSelectIndex.Value = value;
    }

    public int UnlockIndex
    {
        get => SaveSelectIndex.Value;
        set => SaveSelectIndex.Value = value;
    }

    public int UnlockProgress
    {
        get => SaveSelectIndex.Value;
        set => SaveSelectIndex.Value = value;
    }

    public int UnlockCount
    {
        get => SaveUnlockCount.Value;
        set => SaveUnlockCount.Value = value;
    }

    public override void Init()
    {
        base.Init();

        SaveSelectIndex = new sInt(SaveKey + "_" + nameof(SelectIndex), 0);
        SaveUnlockProgress = new sFloat(SaveKey + "_" + nameof(UnlockProgress), 0);
        SaveUnlockIndex = new sInt(SaveKey + "_" + nameof(UnlockIndex), -1);
        SaveUnlockCount = new sInt(SaveKey + "_" + nameof(UnlockCount), 0);

        for (var i = 0; i < Datas.Count; i++)
        {
            var data = Datas[i];
            data.ID = i;
            data.Setting = this;
            data.Init();
        }

        if (UnlockIndex < 0)
        {
            PreNextUnlock();
        }
    }

    public void PreNextUnlock()
    {
        var lockDatas = Datas.FindAll(a => !a.IsUnLock);
        UnlockProgress = 0;
        if (lockDatas.Count > 0)
        {
            UnlockIndex = lockDatas.Random().ID;
        }
        else
        {
            UnlockIndex = -1;
        }
    }
}