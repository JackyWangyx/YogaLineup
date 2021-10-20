using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class AvatarData : StoreData
{
    public new AvatarSetting Setting => base.Setting as AvatarSetting;

    public GameObject Prefab;
    public GameObject PreviewPrefab;

    public override void Init()
    {
        base.Init();
    }
}
