using System;
using System.Collections;
using System.Collections.Generic;
using Aya.Extension;
using Aya.Pool;
using Aya.TweenPro;
using Aya.Util;
using Sirenix.OdinInspector;
using UnityEngine;

[Serializable]
public class UIFlyCoinData
{
    public string Name;
    public Transform Target;
    public GameObject Prefab;
}

public class UIFlyCoin : GameEntity<UIFlyCoin>
{
    public const int Coin = 0;
    public const int Key = 1;

    public float FlyDuration = 1f;
    public float Interval = 0.05f;
    public float RandomStartPos = 100f;
    public int PerFrameLimit = 5;
    public int MaxCount = 100;
    public AnimationCurve CurveX;
    public AnimationCurve CurveY;
    public AnimationCurve CurveScaleCoin;
    public float ScaleTargetDuration;
    public AnimationCurve CurveScaleTarget;

    [TableList] public List<UIFlyCoinData> TargetList;

    public EntityPool CoinPool => PoolManager.Ins["UIFlyCoin"];

    public void Fly(int index, Vector3 startPos, int count, Action onEach = null, Action onDone = null)
    {
        var data = TargetList[index];
        Fly(data.Prefab, startPos, data.Target, count, onEach, onDone);
    }

    public void Fly(GameObject coinPrefab, Vector3 startPos, Transform endTrans, int count, Action onEach = null, Action onDone = null)
    {
        StartCoroutine(FlyCo(coinPrefab, startPos, endTrans, count, onEach, onDone));
    }

    public IEnumerator FlyCo(GameObject coinPrefab, Vector3 startPos, Transform end, int count, Action onEach = null, Action onDone = null)
    {
        for (var i = 0; i < count;)
        {
            while (CoinPool[coinPrefab].SpawnPrefabsCount >= MaxCount)
            {
                yield return null;
            }

            var frameCounter = 0;
            while (frameCounter < PerFrameLimit && i < count)
            {
                var coinStartPos = startPos + RandUtil.RandVector3(-RandomStartPos, RandomStartPos);
                var coinEndPos = end.position;
                var coinIns = CoinPool.Spawn(coinPrefab, transform);
                coinIns.transform.position = coinStartPos;
                UTween.Scale(coinIns.transform, Vector3.zero, Vector3.one, FlyDuration).SetCurve(CurveScaleCoin);
                UTween.Value(0f, 1f, FlyDuration, value =>
                    {
                        var x = Mathf.Lerp(coinStartPos.x, coinEndPos.x, value) * CurveX.Evaluate(value);
                        coinIns.transform.SetPositionX(x);
                        var y = Mathf.Lerp(coinStartPos.y, coinEndPos.y, value) * CurveY.Evaluate(value);
                        coinIns.transform.SetPositionY(y);
                    })
                    .SetOnStop(() =>
                    {
                        onEach?.Invoke();
                        UTween.Scale(end, Vector3.zero, Vector3.one, ScaleTargetDuration).SetCurve(CurveScaleTarget);
                        CoinPool.DeSpawn(coinIns);
                    });

                i++;
                frameCounter++;
            }

            yield return new WaitForSeconds(Interval);
        }

        onDone?.Invoke();
        yield return null;
    }
}
