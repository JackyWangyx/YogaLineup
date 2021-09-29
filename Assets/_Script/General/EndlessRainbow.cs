using System;
using System.Collections.Generic;
using Aya.Extension;
using Aya.TweenPro;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

public class EndlessRainbow : MonoBehaviour
{
    [Serializable]
    public class RainbowData
    {
        public int Index { get; set; }
        [TableColumnWidth(100)]
        public Renderer Renderer;
        [TableColumnWidth(20)]
        public float Value;
    }

    [Serializable]
    public class OnRainbowEvent : UnityEvent<RainbowData>
    {

    }

    [FoldoutGroup("Rainbow")] public Transform RendererTrans;
    [FoldoutGroup("Rainbow")] public int MaterialIndex;
    [FoldoutGroup("Rainbow")] public string MaterialProperty;
    [FoldoutGroup("Rainbow")] public Vector2 MaterialValue;
    [FoldoutGroup("Rainbow")] public float Duration;
    [FoldoutGroup("Rainbow")] public float StartValue = 1f;
    [FoldoutGroup("Rainbow")] public float IntervalValue = 0.2f;
    [FoldoutGroup("Rainbow"), TableList] public List<RainbowData> RainbowList;

    [FoldoutGroup("Event")] public OnRainbowEvent OnRainbow;

    public void Awake()
    {
        for (var i = 0; i < RainbowList.Count; i++)
        {
            var rainbowData = RainbowList[i];
            rainbowData.Index = i;
        }
    }

    public void Animation(int index)
    {
        index = Mathf.Clamp(index, 0, RainbowList.Count - 1);
        var data = RainbowList[index];

        // TODO..
        // UTween.MaterialFloat(data.Renderer, MaterialValue.x, MaterialValue.y, Duration / 2f, 0, MaterialProperty);
        // UTween.MaterialFloat(data.Renderer, MaterialValue.y, MaterialValue.x, Duration / 2f, 0, MaterialProperty).SetStartDelay(Duration / 2f);
        OnRainbow.Invoke(data);
    }

    [Button("Auto Cache"), GUIColor(0.5f, 1f, 0.5f)]
    public void Cache()
    {
        RainbowList = new List<RainbowData>();
        var rendererList = RendererTrans.GetComponentsInChildren<Renderer>().ToList();
        for (var i = 0; i < rendererList.Count; i++)
        {
            var rate = StartValue + IntervalValue * i;
            RainbowList.Add(new RainbowData()
            {
                Renderer = rendererList[i],
                Value = rate
            });
        }
    }
}
