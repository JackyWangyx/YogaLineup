using System;
using Aya.Physical;
using Fishtail.PlayTheBall.Vibration;
using UnityEngine;

public abstract class ItemBase<T> : ItemBase where T : Component
{
    public override Type TargetType => typeof(T);

    public T Target { get; set; }

    public override void CacheComponents()
    {
        base.CacheComponents();

        ColliderListeners.Clear();
        foreach (var col in ColliderList)
        {
            var listener = col.gameObject.GetComponent<ColliderListener>();
            if (listener == null) listener = col.gameObject.AddComponent<ColliderListener>();

            listener.onTriggerEnter.Clear();
            listener.onTriggerEnter.Add<T>(LayerMask, OnEnter);
            listener.onTriggerExit.Clear();
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
            RendererTrans?.gameObject.SetActive(false);
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
