using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UISceneItem : UIBase
{
    public GameEntity Target;
    public Vector3 Offset;

    protected Vector3 CachePosition;

    public virtual void Show(GameEntity target, params object[] args)
    {
        base.Show(args);
        Target = target;
    }

    public override void Hide()
    {
        base.Hide();
        Target = null;
    }

    public virtual void FixedUpdate()
    {
        if (Target == null) return;
        CachePosition = GetFollowLocalPosition(UI.Camera, Trans);
        CachePosition += Offset;
        Rect.localPosition = CachePosition;
    }

    public Vector3 GetFollowLocalPosition(Camera camera, Transform target)
    {
        var position = camera.WorldToScreenPoint(target.position);
        Content.FormatPosition(ref position);
        return position;
    }
}

public abstract class UISceneItem<TTarget> : UISceneItem where TTarget : GameEntity
{
    public new TTarget Target;

    public override void Show(GameEntity target, params object[] args)
    {
        base.Show(args);
        Target = target as TTarget;
    }
}
