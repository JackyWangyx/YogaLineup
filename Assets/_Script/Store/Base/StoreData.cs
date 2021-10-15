using System;
using System.Collections;
using System.Collections.Generic;
using Aya.Data.Persistent;
using UnityEngine;

[Serializable]
public abstract class StoreData
{
    public int ID;
    public Sprite Icon;
    public Sprite UnGetIcon;

    public GameObject Prefab;
    public GameObject PreviewPrefab;

    public bool PreUnlock;
    public bool PreBuy;

    [NonSerialized] public sBool IsUnLock;
    [NonSerialized] public sBool IsBuy;

    public static sInt SelectIndex;
    public static sFloat UnlockProgress;
    public static sInt UnlockIndex;

    static StoreData()
    {
        var name = System.Reflection.MethodBase.GetCurrentMethod().ReflectedType.Name;
        SelectIndex = new sInt(name + "_" + nameof(SelectIndex));
        UnlockProgress = new sFloat(name + "_" + nameof(UnlockProgress));
        UnlockIndex = new sInt(name + "_" + nameof(UnlockIndex));
    }

    public virtual void Init()
    {
        var name = GetType().Name;
        IsUnLock = new sBool(name + "_" + ID + "_Unlock", PreUnlock);
        IsBuy = new sBool(name + "_" + ID + "_Buy", PreBuy);
    }

    public virtual bool Unlock()
    {
        if (!IsUnLock)
        {
            IsUnLock.Value = true;
            return true;
        }

        return false;
    }

    public void Select()
    {
        // SaveManager.Ins.SelectAvatarID.Value = ID;
    }

    public virtual bool Buy()
    {
        // if (Enough && !IsBuy)
        // {
        //     SaveManager.Ins.Coin.Value -= CurrentCost;
        //     SaveManager.Ins.CurrentUnlockAvatarCount.Value++;
        //     IsBuy.Value = true;
        //     return true;
        // }

        return false;
    }
}
