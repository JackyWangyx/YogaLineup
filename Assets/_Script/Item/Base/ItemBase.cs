using System;
using System.Collections.Generic;
using UnityEngine;
using Aya.Physical;
using Aya.Extension;
using MoreMountains.NiceVibrations;
using Sirenix.OdinInspector;

public abstract class ItemBase : GameEntity
{
    public List<Collider> ColliderList { get; set; }
    public List<ColliderListener> ColliderListeners { get; set; }
    public Animator Animator { get; set; }
    public virtual Type TargetType { get; set; }

    [FoldoutGroup("Pram")] public LayerMask LayerMask;
    [FoldoutGroup("Pram")] public bool DeSpawnAfterEffect = true;
    [FoldoutGroup("Pram")] public bool DeActiveRender;
    [FoldoutGroup("Pram")] public bool EffectiveOnce = true;
    [FoldoutGroup("Pram")] public List<GameObject> RenderPrefabs;

    [FoldoutGroup("Effect")] public GameObject EffectSelfFx;
    [FoldoutGroup("Effect")] public GameObject EffectTargetFx;

    [FoldoutGroup("Animator")] public string DefaultClip;
    [FoldoutGroup("Animator")] public string EffectClip;

    [FoldoutGroup("Exclude")] public List<ItemBase> ExcludeItems;

    [FoldoutGroup("Vibration")] public HapticTypes VibrationType = HapticTypes.None;

    public bool Active { get; set; }
    public GameObject RenderInstance { get; set; }
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
        if (Animator != null && !string.IsNullOrEmpty(DefaultClip))
        {
            Animator.Play(DefaultClip);
        }

        Active = true;
    }

    public virtual void InitRenderPrefab()
    {
        if (RenderInstance != null)
        {
            GamePool.DeSpawn(RenderInstance);
        }

        if (RenderPrefabs != null && RenderPrefabs.Count > 0)
        {
            var prefab = RenderPrefabs.Random();
            RenderInstance = GamePool.Spawn(prefab, RendererTrans);
        }
    }

    public virtual void CacheComponents()
    {
        Animator = GetComponentInChildren<Animator>();
        ColliderList = GetComponentsInChildren<Collider>().ToList();
        ColliderListeners = new List<ColliderListener>();
    }
}