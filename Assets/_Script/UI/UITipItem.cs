using System.Collections;
using System.Collections.Generic;
using Aya.Extension;
using Aya.TweenPro;
using UnityEngine;
using UnityEngine.UI;

public class UITipItem : MonoBehaviour
{
    public Text Text;
    public float Duration;

    public UITip UiTip => UITip.Ins;
    public RectTransform Rect { get; set; }

    public void Awake()
    {
        Rect = GetComponent<RectTransform>();
    }

    public void Show(string text)
    {
        Text.text = text;
    }

    public void Show(string text, Color color)
    {
        Text.color = color;
        Show(text);
        UiTip.ExecuteDelay(() =>
        {
            UiTip.Pool.DeSpawn(this);
        }, Duration);
    }
}
