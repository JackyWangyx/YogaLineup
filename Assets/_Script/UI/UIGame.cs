using System.Collections;
using System.Collections.Generic;
using Aya.UI;
using UnityEngine;
using UnityEngine.UI;

public class UIGame : UIBase<UIGame>
{
    public UILevelProgress LevelProgress;

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
}
