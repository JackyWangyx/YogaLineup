using System;
using Aya.Extension;
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
                foreach (var condition in Conditions)
                {
                    var check = condition.CheckCondition(target);
                    if (!check) return;
                }

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

        foreach (var go in ActiveList)
        {
            go?.SetActive(true);
        }

        foreach (var go in DeActiveList)
        {
            go?.SetActive(false);
        }

        if (SelfFx != null && SelfFx.Count > 0)
        {
            foreach (var fx in SelfFx)
            {
                SpawnFx(fx);
            }
        }

        if (SelfRandomFx != null && SelfRandomFx.Count > 0)
        {
            var fx = SelfRandomFx.Random();
            SpawnFx(fx);
        }

        var targetFxTrans = target.transform;
        if (target is Player player) targetFxTrans = player.RenderTrans;
        if (TargetFx != null && TargetFx.Count > 0)
        {
            foreach (var fx in TargetFx)
            {
                SpawnFx(fx, targetFxTrans);
            }
        }

        if (TargetRandomFx != null && TargetRandomFx.Count > 0)
        {
            var fx = TargetRandomFx.Random();
            SpawnFx(fx, targetFxTrans);
        }

        foreach (var animatorData in AnimatorDataList)
        {
            animatorData.Animator?.Play(animatorData.Clip);
        }

        foreach (var tweenAnimation in TweenAnimationList)
        {
            tweenAnimation?.Data.Play();
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
