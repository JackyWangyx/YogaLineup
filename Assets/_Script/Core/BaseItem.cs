using System;
using System.Collections.Generic;
using System.Linq;
using Aya.Particle;
using UnityEngine;
using Aya.Physical;
using Aya.Pool;

public abstract class BaseItem : MonoBehaviour
{
    public Transform Render { get; set; }
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
    public GameObject EffectFx;
    [Header("Animator")]
    public string DefaultClip;
    public string EffectClip;
    [Header("Exclude")]
    public List<BaseItem> ExcludeItems;

    public bool Active { get; set; }
    public virtual bool IsUseful => true;

    public virtual void Awake()
    {
        Render = transform.Find("Render");
        Animator = GetComponentInChildren<Animator>();
        ColliderList = GetComponentsInChildren<Collider>().ToList();
        ColliderListeners = new List<ColliderListener>();

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


    public override void Awake()
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

        if (EffectiveOnce)
        {
            Active = false;
        }
        
        if (DeActiveRender)
        {
            Render.gameObject.SetActive(false);
        }

        if (EffectFx != null)
        {
            ParticleSpawner.Spawn(EffectFx, PoolManager.Ins.transform, transform.position);
        }

        if (Animator != null && !string.IsNullOrEmpty(EffectClip))
        {
            Animator.Play(EffectClip);
        }

        if (DeSpawnAfterEffect)
        {
            Destroy(gameObject);
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
