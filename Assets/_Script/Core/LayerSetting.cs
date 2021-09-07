using System.Collections;
using System.Collections.Generic;
using Aya.Singleton;
using UnityEngine;

public class LayerSetting : GameEntity<LayerSetting>
{
    public LayerMask Player;
    public LayerMask Item;
    public LayerMask Road;
}
