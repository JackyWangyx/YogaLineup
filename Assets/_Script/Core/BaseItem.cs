using System.Collections.Generic;
using System.Linq;
using Aya.Particle;
using UnityEngine;
using Aya.Physical;
using Aya.Pool;


public abstract class BaseItem<T> : MonoBehaviour where T : Component
{
    public Transform Render { get; set; }
    public List<Collider> ColliderList { get; set; }
    public List<ColliderListener> ColliderListeners { get; set; }
    public LayerMask LayerMask;
    public bool DeSpawnAfterEffect = true;
    public bool DeActiveRender;
    public bool EffectiveOnce = true;
    public GameObject EffectFx;

    public bool Active { get; set; }

    public abstract bool IsUseful { get; }
    public T Target { get; set; } 

    public virtual void Awake()
    {
        Render = transform.Find("Render");
        ColliderList = GetComponentsInChildren<Collider>().ToList();
        ColliderListeners = new List<ColliderListener>();
        foreach (var col in ColliderList)
        {
            var listener = col.gameObject.GetComponent<ColliderListener>();
            if(listener == null) listener = col.gameObject.AddComponent<ColliderListener>();

            listener.onTriggerEnter.Add<T>(LayerMask, OnEnter);
            listener.onTriggerExit.Add<T>(LayerMask, OnExit);
            ColliderListeners.Add(listener);
        }

        Active = true;
    }

    public virtual void OnEnter(T target)
    {
        if (!Active) return;
        if (Active)
        {
            Target = target;
            OnTargetEnter(target);
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
