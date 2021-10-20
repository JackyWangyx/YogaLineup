using System;
using Aya.Data.Persistent;
using Aya.Extension;
using UnityEngine;

[Serializable]
public abstract class StoreData
{
    public int ID { get; set; }
    public Sprite Icon;
    public Sprite UnGetIcon;

    public bool PreUnlock;
    public bool PreBuy;

    public IStoreSetting<StoreData> Setting { get; set; }

    [NonSerialized] public sBool IsUnLock;
    [NonSerialized] public sBool IsBuy;

    public bool CanUnlock => Setting.UnlockProgress >= 100;
    public bool IsSelected => Setting.SelectIndex == ID;
    public bool IsCoinEnough => SaveManager.Ins.Coin >= Setting.CurrentCost;

    public virtual void Init()
    {
        var name = GetType().Name;
        IsUnLock = new sBool(name + "_" + ID + "_Unlock", PreUnlock);
        IsBuy = new sBool(name + "_" + ID + "_Buy", PreBuy);
    }

    public virtual bool Unlock()
    {
        if (IsUnLock) return false;
        IsUnLock.Value = true;
        Setting.PreNextUnlock();
        return true;
    }

    public virtual void Select()
    {
        Setting.SelectIndex = ID;
    }

    public virtual bool Buy(bool needCost = true)
    {
        if (IsBuy) return false;
        if (needCost)
        {
            if (IsCoinEnough)
            {
                SaveManager.Ins.Coin.Value -= Setting.CurrentCost;
                Setting.UnlockCount++;
            }
            else
            {
                return false;
            }
        }

        IsBuy.Value = true;
        return true;
    }
}
