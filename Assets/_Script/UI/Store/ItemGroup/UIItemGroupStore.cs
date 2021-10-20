using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIItemGroupStore : UIStoreBase<UIItemGroupStore, UIItemGroupItem, ItemGroupData>
{
    public override List<ItemGroupData> DataSources => ItemGroupSetting.Ins.Datas;
}
