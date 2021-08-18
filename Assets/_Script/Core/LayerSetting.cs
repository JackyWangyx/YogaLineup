using System.Collections;
using System.Collections.Generic;
using Aya.Singleton;
using UnityEngine;

public class LayerSetting : MonoSingleton<GameManager>
{
    public LayerMask Player;
    public LayerMask Item;
}
