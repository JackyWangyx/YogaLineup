using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UIStoreBase<TUI, TItem, TData> : UIBase<TUI>
    where TUI : UIStoreBase<TUI, TItem, TData>
    where TItem : UIStoreItemBase<TData>
    where TData : StoreData
{
    public IStoreSetting<TData> Setting { get; set; }
    public TItem ItemPrefab;
    public Transform ItemTrans;

    public List<TItem> ItemInsList { get; set; } = new List<TItem>();

    public abstract List<TData> DataSources { get; }

    public override void Show()
    {
        base.Show();

        foreach (var uiShopItem in ItemInsList)
        {
            UIPool.DeSpawn(uiShopItem.gameObject);
        }

        ItemInsList.Clear();

        foreach (var avatarData in DataSources)
        {
            var item = UIPool.Spawn(ItemPrefab, ItemTrans);
            item.Init(avatarData);
            ItemInsList.Add(item);
        }

        Refresh();
    }

    public virtual void Refresh()
    {
        foreach (var item in ItemInsList)
        {
            item.Refresh();
        }
    }
}
