﻿using System;
using System.Collections.Generic;
using UnityEngine;
using Aya.Physical;
using Aya.Extension;
using MoreMountains.NiceVibrations;
using Sirenix.OdinInspector;

[Serializable]
public class ItemAnimatorData
{
    public Animator Animator;
    public string Clip;
    public string DefaultClip;
}

public abstract class ItemBase : GameEntity
{
    [FoldoutGroup("Pram")] public LayerMask LayerMask;
    [FoldoutGroup("Pram")] public bool DeSpawnAfterEffect = true;
    [FoldoutGroup("Pram")] public bool DeActiveRender;
    [FoldoutGroup("Pram")] public bool EffectiveOnce = true;
    [FoldoutGroup("Pram")] public List<GameObject> RenderPrefabs;
    [FoldoutGroup("Pram")] public List<GameObject> RenderRandomPrefabs;

    [FoldoutGroup("Effect")] public List<GameObject> SelfFx;
    [FoldoutGroup("Effect")] public List<GameObject> SelfRandomFx;
    [FoldoutGroup("Effect")] public List<GameObject> TargetFx;
    [FoldoutGroup("Effect")] public List<GameObject> TargetRandomFx;

    [FoldoutGroup("Animator"), TableList] public List<ItemAnimatorData> AnimatorDataList;
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