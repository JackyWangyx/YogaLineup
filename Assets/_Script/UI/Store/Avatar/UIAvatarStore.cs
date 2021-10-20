using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIAvatarStore : UIStoreBase<UIAvatarStore, UIAvatarItem, AvatarData>
{
    public override List<AvatarData> DataSources => AvatarSetting.Ins.Datas;
}
