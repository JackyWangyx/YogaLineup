using System;
using System.Collections;
using System.Collections.Generic;
using Aya.UI;
using UnityEngine;
using UnityEngine.UI;

public class UIGame : UIBase<UIGame>
{
    public UILevelProgress LevelProgress;

    public Transform FlyCoinStart;
    public Transform FlyCoinEnd;

    protected override void Awake()
    {
        base.Awake();
    }

    public override void Show()
    {
        base.Show();
    }

    public void Update()
    {

    }

    public void Retry()
    {
        Level.LevelStart();
    }

    public void FlyCoin(GameObject prefab, int count, Action action = null)
    {
        UIFlyCoin.Ins.Fly(prefab, FlyCoinStart.position, FlyCoinEnd.position, count, action);
    }
}
