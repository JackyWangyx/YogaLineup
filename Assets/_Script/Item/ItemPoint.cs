using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ItemPoint : BaseItem<Player>
{
    [Header("Point")]
    public int AddValue;
    public float MultiplyValue = 1f;
    public TMP_Text Text;
    public bool ShowTip;
    public Color GoodTipColor;
    public Color BadTipColor;

    public string TextValue { get; set; }

    public override void Awake()
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

    public override void OnTargetEnter(Player target)
    {
        var value = (int)(target.Point * MultiplyValue + AddValue);
        var diff = value - target.Point;
        target.ChangePoint(diff);

        if (ShowTip)
        {
            UITip.Ins.ShowTipWithWorldPos(TextValue, diff > 0 ? GoodTipColor : BadTipColor, transform.position);
        }
    }

    public override void OnTargetExit(Player target)
    {
       
    }
}
