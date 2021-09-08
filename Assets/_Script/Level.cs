using System.Collections.Generic;
using UnityEngine;
using Aya.Extension;
using Aya.Simplify;
using Dreamteck.Splines;

public class Level : GameEntity
{
    public List<BaseItem> ItemList { get; set; }
    public SplineComputer Path;
    public float Width;
    public float HalfWidth => Width / 2f;
    public Vector2 TurnRange => new Vector2(-HalfWidth, HalfWidth);

    protected override void Awake()
    {
        base.Awake();
        ItemList = transform.GetComponentsInChildren<BaseItem>().ToList();
    }

    public void Init()
    {
        foreach (var item in ItemList)
        {
            item.Init();
        }

        Player.Init();
    }
}
