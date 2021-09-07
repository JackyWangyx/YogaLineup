using System;
using Aya.Extension;
using Aya.TweenPro;
using Aya.Util;
using UnityEngine;

public class UIFlyCoin : GameEntity<UIFlyCoin>
{
    public float FlyDuration = 1f;
    public float RandomDelay = 1f;
    public float RandomStartPos = 5f;
    public AnimationCurve CurveX;
    public AnimationCurve CurveY;

    public void Fly(GameObject prefab, Vector3 startPos, Vector3 endPos, int count, Action onDone = null)
    {
        for (var i = 0; i < count; i++)
        {
            var delay = RandUtil.RandFloat(0f, RandomDelay);
            this.ExecuteDelay(() =>
            {
                var flyStartPos = startPos + RandUtil.RandVector3(new Vector3(-RandomStartPos, -RandomStartPos, 0f), new Vector3(RandomStartPos, RandomStartPos, 0f));
                var flyEndPos = endPos;
                var instance = GamePool.Spawn(prefab, transform);
                UTween.Value(flyStartPos.x, flyEndPos.x, FlyDuration, value =>
                    {
                        instance.transform.SetPositionX(value);
                    })
                    .SetCurve(CurveX);

                UTween.Value(flyStartPos.y, flyEndPos.y, FlyDuration, value =>
                    {
                        instance.transform.SetPositionX(value);
                    })
                    .SetCurve(CurveY)
                    .SetOnStop(() =>
                    {
                        GamePool.DeSpawn(instance);
                    });

            }, delay);
        }

        this.ExecuteDelay(() =>
        {
            onDone?.Invoke();
        }, FlyDuration * RandomDelay);
    }
}
