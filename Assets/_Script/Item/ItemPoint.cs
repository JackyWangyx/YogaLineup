using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

public class ItemPoint : ItemBase<Player>
{
    [BoxGroup("Point")] public int AddValue;
    [BoxGroup("Point")] public float MultiplyValue = 1f;
    [BoxGroup("Point")] public TMP_Text Text;
    [BoxGroup("Point")] public bool ShowTip;
    [BoxGroup("Point")] public Color GoodTipColor;
    [BoxGroup("Point")] public Color BadTipColor;

    public string TextValue { get; set; }

    protected override void Awake()
    {
        base.Awake();

        if (AddValue != 0)
        {
            if (AddValue > 0) TextValue = "＋" + AddValue;
            else TextValue = "－" + Mathf.Abs(AddValue);
        }
        else
        {
            if (MultiplyValue > 1f)
            {
                TextValue = "×" + MultiplyValue;
            }
            else
            {
                var value = 1f / MultiplyValue;
                TextValue = "÷" + value;
            }
        }

        if (Text != null)
        {
            Text.text = TextValue;
        }
    }

    public override void OnTargetEffect(Player target)
    {
        var value = (int)(target.State.Point * MultiplyValue + AddValue);
        var diff = value - target.State.Point;

        if (diff < 0 && target.State.IsInvincible) return;
        target.State.ChangePoint(diff);

        if (ShowTip)
        {
            UITip.Ins.ShowTip(transform.position).Set(TextValue, diff > 0 ? GoodTipColor : BadTipColor);
        }
    }
}
