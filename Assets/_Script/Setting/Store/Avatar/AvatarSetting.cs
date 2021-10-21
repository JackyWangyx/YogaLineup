using System;
using System.Collections;
using System.Collections.Generic;
using Aya.Data.Persistent;
using UnityEngine;

[CreateAssetMenu(fileName = "AvatarSetting", menuName = "Setting/Avatar Setting")]
public class AvatarSetting : StoreSetting<AvatarSetting, AvatarData>
{
    [NonSerialized] public sInt AvatarIndex;
    public List<GameObject> SelectedAvatarList => Datas[AvatarIndex].Prefabs;

    public override void Init()
    {
        base.Init();

        AvatarIndex = new sInt(nameof(AvatarIndex), 0);
    }
}
