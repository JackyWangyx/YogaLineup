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

    public override void Awake()
    {
        base.Awake();

        if (Text != null)
        {
            if (AddValue != 0)
            {
                if (AddValue > 0) Text.text = "＋" + AddValue;
                else Text.text = "－" + Mathf.Abs(AddValue);
            }
            else
            {
                if (MultiplyValue > 1f)
                {
                    Text.text = "×" + MultiplyValue;
                }
                else
                {
                    var value = 1f / MultiplyValue;
                    Text.text = "÷" + value;
                }
            }
        }
    }

    public override void OnTargetEnter(Player target)
    {
        var value = (int)(target.Point * MultiplyValue + AddValue);
        var diff = value - target.Point;
        if (diff > 0)
        {
            target.Point += diff;
        }
        else
        {
            diff = -diff;
            target.Point -= diff;
        }

        if (target.Point < 0) target.Point = 0;
        target.RefreshRender(target.Point);
    }

    public override void OnTargetExit(Player target)
    {
       
    }
}
