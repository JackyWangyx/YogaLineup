using System.Collections;
using System.Collections.Generic;
using Aya.Extension;
using UnityEngine;
using UnityEngine.UI;

public class UILevelProgress : GameEntity
{
    public Text TextCurrent;
    public Text TextNext;
    public Image Progress;
    public Image ProgressIndicator;
    public Text TextPoint;

    private RectTransform _barRect;
    private RectTransform _indicatorRect;

    protected override void Awake()
    {
        base.Awake();
        _barRect = Progress.GetComponent<RectTransform>();
        _indicatorRect = ProgressIndicator.GetComponent<RectTransform>();
    }

    public void Update()
    {
        TextCurrent.text = Save.LevelIndex.Value.ToString();
        TextNext.text = (Save.LevelIndex.Value + 1).ToString();
        TextPoint.text = Game.Player.State.Point.ToString();
        TextPoint.color = Game.Player.Data.Color;

        var factor = Player.PathFollower.Factor;
        Progress.fillAmount = Mathf.Lerp(Progress.fillAmount, factor, Time.deltaTime * 2f);

        var width = _barRect.GetSize().x;
        var x = -width / 2f + width * Progress.fillAmount;
        var pos = _indicatorRect.anchoredPosition;
        pos.x = x;
        _indicatorRect.anchoredPosition = pos;
    }
}
