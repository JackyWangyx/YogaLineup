using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UIStoreItemBase<TData> : GameEntity
    where TData : StoreData
{
    public TData Data { get; set; }

    public virtual void Init(TData data)
    {
        Data = data;
    }

    public abstract void Refresh();
}