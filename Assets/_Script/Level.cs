using System.Collections.Generic;
using UnityEngine;
using Aya.Extension;
using Aya.Simplify;
using Dreamteck.Splines;

public class Level : MonoBehaviour
{
    public SplineComputer Path;
    public float Width;
    public float HalfWidth => Width / 2f;
    public Vector2 TurnRange => new Vector2(-HalfWidth, HalfWidth);

    public void Init()
    {
        GameManager.Ins.Player.Init();
    }
}
