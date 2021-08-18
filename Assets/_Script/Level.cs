using System.Collections.Generic;
using UnityEngine;
using Aya.Extension;
using Aya.Simplify;
using Dreamteck.Splines;

public class Level : MonoBehaviour
{
    public SplineComputer Path;
    public float Width;

    public void Init()
    {
        GameManager.Ins.Player.Init();
    }
}
