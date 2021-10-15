using System.Collections;
using System.Collections.Generic;
using Aya.UI;
using UnityEngine;
using UnityEngine.UI;

public class UIGame : UIBase<UIGame>
{
    public GameObject TouchTip;
    public GameObject TouchStartRunArea;
    public UILevelProgress LevelProgress;

    private bool _clickStartRun;

    protected override void Awake()
    {
        base.Awake();
        UIEventListener.Get(TouchStartRunArea).onDown += (go, data) =>
        {
            if (!_clickStartRun)
            {
                _clickStartRun = true;
                Player.StartRun();
                TouchTip.gameObject.SetActive(false);
            }
        };
    }

    public override void Show()
    {
        base.Show();
        _clickStartRun = false;
        TouchTip.gameObject.SetActive(true);
    }

    public void Update()
    {

    }

    public void Retry()
    {
        Level.LevelStart();
    }
}
