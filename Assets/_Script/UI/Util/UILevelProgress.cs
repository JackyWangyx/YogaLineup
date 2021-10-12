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
    public Text TextHp;

    private RectTransform _barRect;
    private RectTransform _indicatorRect;

    protected override void Awake()
    {
        base.Awake();
        _barRect = Progress.GetComponent<RectTransform>();
        _indicatorRect = ProgressIndicator.GetComponent<RectTransform>();
    }

    void Update()
    {
        TextCurrent.text = SaveManager.Ins.LevelIndex.Value.ToString();
        TextNext.text = (SaveManager.Ins.LevelIndex.Value + 1).ToString();
        TextHp.text = GameManager.Ins.Player.State.Point.ToString();
        TextHp.color = GameManager.Ins.Player.Data.Color;

        var level = CurrentLevel;
        var blockValue = 1f / level.BlockList.Count;
        var factor = Player.PathFollower.Factor;
        // var factor = level.CurrentBlockIndex * blockValue + blockValue * level.CurrentBlock.Path.MoveDistance / level.CurrentBlock.Path.Length;
        Progress.fillAmount = factor;

        var width = _barRect.GetSize().x;
        var x = -width / 2f + width * Progress.fillAmount;
        var pos = _indicatorRect.anchoredPosition;
        pos.x = x;
        _indicatorRect.anchoredPosition = pos;
    }
}
