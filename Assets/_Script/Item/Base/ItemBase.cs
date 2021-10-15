using System;
using System.Collections.Generic;
using UnityEngine;
using Aya.Physical;
using Aya.Extension;
using Aya.TweenPro;
using MoreMountains.NiceVibrations;
using Sirenix.OdinInspector;


public abstract class ItemBase : GameEntity
{
    [FoldoutGroup("Pram")] public LayerMask LayerMask;
    [FoldoutGroup("Pram")] public bool DeSpawnAfterEffect = true;
    [FoldoutGroup("Pram")] public bool DeActiveRender;
    [FoldoutGroup("Pram")] public bool EffectiveOnce = true;
    [FoldoutGroup("Pram")] public List<GameObject> RenderPrefabs;
    [FoldoutGroup("Pram")] public List<GameObject> RenderRandomPrefabs;

    [FoldoutGroup("Condition"), SerializeReference] public List<ItemCondition> Conditions = new List<ItemCondition>();

    [FoldoutGroup("Active")] public List<GameObject> ActiveList;
    [FoldoutGroup("Active")] public List<GameObject> DeActiveList;

    [FoldoutGroup("Effect")] public List<GameObject> SelfFx;
    [FoldoutGroup("Effect")] public List<GameObject> SelfRandomFx;
    [FoldoutGroup("Effect")] public List<GameObject> TargetFx;
    [FoldoutGroup("Effect")] public List<GameObject> TargetRandomFx;

    [FoldoutGroup("Animation")] public List<UTweenAnimation> TweenAnimationList;
    [FoldoutGroup("Animation"), TableList] public List<ItemAnimatorData> AnimatorDataList;
    [FoldoutGroup("Exclude")] public List<ItemBase> ExcludeItems;
    [FoldoutGroup("Vibration")] public HapticTypes VibrationType = HapticTypes.None;

    public List<Collider> ColliderList { get; set; }
    public List<ColliderListener> ColliderListeners { get; set; }
    public Animator Animator { get; set; }
    public virtual Type TargetType { get; set; }

    public bool Active { get; set; }
    public List<GameObject> RenderInstanceList { get; set; }
    public virtual bool IsUseful => true;

    protected override void Awake()
    {
        base.Awake();
    }

    public virtual void Init()
    {
        InitRenderPrefab();
        CacheComponents();

        gameObject.SetActive(true);
        RendererTrans?.gameObject.SetActive(true);
        foreach (var animatorData in AnimatorDataList)
        {
            animatorData.Animator?.Play(animatorData.DefaultClip);
        }

        foreach (var tweenAnimation in TweenAnimationList)
        {
            tweenAnimation.Data.Sample(0f);
        }

        foreach (var go in ActiveList)
        {
            go?.SetActive(false);
        }

        foreach (var go in DeActiveList)
        {
            go?.SetActive(true);
        }

        Active = true;
    }

    public virtual void InitRenderPrefab()
    {
        if (RenderInstanceList != null && RenderInstanceList.Count > 0)
        {
            foreach (var ins in RenderInstanceList)
            {
                GamePool.DeSpawn(ins);
            }
        }

        RenderInstanceList = new List<GameObject>();
        if (RenderPrefabs != null && RenderPrefabs.Count > 0)
        {
            foreach (var prefab in RenderPrefabs)
            {
                var ins = GamePool.Spawn(prefab, RendererTrans);
                RenderInstanceList.Add(ins);
            }
        }

        if (RenderRandomPrefabs != null && RenderRandomPrefabs.Count > 0)
        {
            var prefab = RenderRandomPrefabs.Random();
            var ins = GamePool.Spawn(prefab, RendererTrans);
            RenderInstanceList.Add(ins);
        }
    }

    public virtual void CacheComponents()
    {
        Animator = GetComponentInChildren<Animator>();
        ColliderList = GetComponentsInChildren<Collider>().ToList();
        ColliderListeners = new List<ColliderListener>();
    }
}