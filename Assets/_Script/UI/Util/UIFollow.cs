using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIFollow : GameEntity
{
    public new Camera Camera;
    public Transform Target;
    public RectTransform Ui;
    public Vector3 Offset;

    private Vector3 _position;

    protected override void Awake()
    {
        base.Awake();
        if (Camera == null)
        {
            Camera = Camera.main;
        }

        if (Ui == null)
        {
            Ui = GetComponent<RectTransform>();
        }
    }

    private void FixedUpdate()
    {
        _position = GetFollowLocalPosition(Camera, Target);
        _position += Offset;
        Ui.localPosition = _position;
    }

    public static Vector3 GetFollowLocalPosition(Camera camera, Transform target)
    {
        var position = camera.WorldToScreenPoint(target.position);
        Content.FormatPosition(ref position);
        return position;
    }
}

public class Content
{
    public const float UiWidth = 1080f;
    public const float UiHeight = 1920f;

    public static float ScreenWidthHalf = Screen.width / 2f;
    public static float ScreenHeightHalf = Screen.height / 2f;

    public static float ScreenWidthRatio = UiWidth / Screen.width;
    public static float ScreenHeightRatio = UiHeight / Screen.height;

    public static void FormatPosition(ref Vector3 pos)
    {
        pos.x -= Content.ScreenWidthHalf;
        pos.y -= Content.ScreenHeightHalf;
        pos.x *= Content.ScreenWidthRatio;
        pos.y *= Content.ScreenHeightRatio;
        pos.z = 0f;
    }
}