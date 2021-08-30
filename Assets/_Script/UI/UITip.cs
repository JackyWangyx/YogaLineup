using Aya.Extension;
using Aya.Pool;
using Aya.Singleton;
using UnityEngine;
using UnityEngine.UI;

public class UITip : MonoSingleton<UITip>
{
    public Camera Camera;
    public UITipItem TipPrefab;
    public float DeSpawnDuration;

    public EntityPool Pool => PoolManager.Ins["Tip"];
    public RectTransform Rect { get; set; }

    protected override void Awake()
    {
        base.Awake();
        Rect = GetComponent<RectTransform>();
    }

    public void ShowTipWithWorldPos(string text, Vector3 worldPosition)
    {
        var pos = WorldPointToUiLocalPoint(worldPosition, Camera, Rect);
        var tip = Pool.Spawn(TipPrefab, transform);
        tip.Rect.anchoredPosition = pos;
        tip.Show(text);
    }

    public void ShowTipWithWorldPos(string text, Color color, Vector3 worldPosition)
    {
        var pos = WorldPointToUiLocalPoint(worldPosition, Camera, Rect);
        var tip = Pool.Spawn(TipPrefab, transform);
        tip.Rect.anchoredPosition = pos;
        tip.Show(text, color);
    }

    public void ShowTip(string text, Vector3 uiPosition)
    {
        var tip = Pool.Spawn(TipPrefab, transform, uiPosition);
        tip.Show(text);
    }

    public void ShowTip(string text, Color color, Vector3 uiPosition)
    {
        var tip = Pool.Spawn(TipPrefab, transform, uiPosition);
        tip.Show(text, color);
    }

    private Vector2 WorldPointToUiLocalPoint(Vector3 point, Camera uiCamera, RectTransform rect)
    {
        var screenPoint = uiCamera.WorldToScreenPoint(point);
        RectTransformUtility.ScreenPointToLocalPointInRectangle(rect, screenPoint, uiCamera, out var localPoint);
        return localPoint;
    }
}
