using System;
using System.Collections.Generic;
using UnityEngine;
using Aya.Physical;
using Aya.Extension;
using Fishtail.PlayTheBall.Vibration;
using MoreMountains.NiceVibrations;

public abstract class BaseItem : GameEntity
{
    public Transform Renderer { get; set; }
    public List<Collider> ColliderList { get; set; }
    public List<ColliderListener> ColliderListeners { get; set; }
    public Animator Animator { get; set; }
    public virtual Type TargetType { get; set; }

    [Header("Pram")]
    public LayerMask LayerMask;
    public bool DeSpawnAfterEffect = true;
    public bool DeActiveRender;
    public bool EffectiveOnce = true;
    [Header("Effect")]
    public GameObject EffectSelfFx;
    public GameObject EffectTargetFx;
    [Header("Animator")]
    public string DefaultClip;
    public string EffectClip;
    [Header("Exclude")]
    public List<BaseItem> ExcludeItems;

    [Header("Vibration")]
    public HapticTypes VibrationType = HapticTypes.None;

    public bool Active { get; set; }
    public virtual bool IsUseful => true;

    protected override void Awake()
    {
        base.Awake();

        Renderer = transform.FindInAllChild("Renderer");
        Animator = GetComponentInChildren<Animator>();
        ColliderList = GetComponentsInChildren<Collider>().ToList();
        ColliderListeners = new List<ColliderListener>();
    }

    public virtual void Init()
    {
        gameObject.SetActive(true);
        Renderer?.gameObject.SetActive(true);
        if (Animator != null && !string.IsNullOrEmpty(DefaultClip))
        {
            Animator.Play(DefaultClip);
        }

        Active = true;
    }
}

public abstract class BaseItem<T> : BaseItem where T: Component
{
    public override Type TargetType => typeof(T);

    public T Target { get; set; }

    protected override void Awake()
    {
        base.Awake();

        foreach (var col in ColliderList)
        {
            var listener = col.gameObject.GetComponent<ColliderListener>();
            if(listener == null) listener = col.gameObject.AddComponent<ColliderListener>();

            listener.onTriggerEnter.Add<T>(LayerMask, OnEnter);
            listener.onTriggerExit.Add<T>(LayerMask, OnExit);
            ColliderListeners.Add(listener);
        }
    }

    public virtual void OnEnter(T target)
    {
        if (!Active) return;
        if (Active)
        {
            try
            {
                Target = target;
                foreach (var item in ExcludeItems)
                {
                    item.Active = false;
                }

                OnTargetEnter(target);
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            } 
        }

        VibrationController.Instance.Impact(VibrationType);

        if (EffectiveOnce)
        {
            Active = false;
        }
        
        if (DeActiveRender)
        {
            Renderer?.gameObject.SetActive(false);
        }

        if (EffectSelfFx != null)
        {
            SpawnFx(EffectSelfFx);
        }

        if (EffectTargetFx != null)
        {
            SpawnFx(EffectTargetFx, target.transform);
        }

        if (Animator != null && !string.IsNullOrEmpty(EffectClip))
        {
            Animator.Play(EffectClip);
        }

        if (DeSpawnAfterEffect)
        {
            gameObject.SetActive(false);
        }
    }

    public virtual void OnExit(T target)
    {
        OnTargetExit(target);
        Target = null;
    }

    public abstract void OnTargetEnter(T target);
    public abstract void OnTargetExit(T target);
}
