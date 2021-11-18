using System;
using Aya.Extension;
using Aya.Physical;
using Fishtail.PlayTheBall.Vibration;
using UnityEngine;

public abstract class ItemBase<TTarget> : ItemBase 
    where TTarget : Component
{
    public override Type TargetType => typeof(TTarget);

    public TTarget Target { get; set; }

    public override void CacheComponents()
    {
        base.CacheComponents();

        foreach (var col in ColliderList)
        {
            var listener = col.gameObject.GetComponent<ColliderListener>();
            if (listener == null) listener = col.gameObject.AddComponent<ColliderListener>();

            listener.Clear();
            listener.onTriggerEnter.Clear();
            listener.onTriggerEnter.Add<TTarget>(LayerMask, OnEnter);
            listener.onTriggerExit.Clear();
            listener.onTriggerExit.Add<TTarget>(LayerMask, OnExit);

            ColliderListeners.Add(listener);
        }
    }

    public virtual void OnEnter<T>(T target) where T : Component
    {
        if (!Active) return;

        EffectCounter++;
        if (EffectMode == ItemEffectMode.Once)
        {
            Active = false;
        }
        else if (EffectMode == ItemEffectMode.Count && EffectCount > 0)
        {
            if (EffectCounter > EffectCount)
            {
                Active = false;
            }
            else
            {
                return;
            }
        }

        try
        {
            Target = target as TTarget;
            foreach (var condition in Conditions)
            {
                var check = condition.CheckCondition(target);
                if (!check) return;
            }

            foreach (var item in ExcludeItems)
            {
                item.Active = false;
            }

            OnTargetEnter(target as TTarget);
        }
        catch (Exception e)
        {
            Debug.LogError(e);
        }

        VibrationController.Instance.Impact(VibrationType);

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

        if (DeSpawnMode == ItemDeSpawnMode.Effect)
        {
            gameObject.SetActive(false);
        }
    }

    public virtual void OnExit<T>(T target) where T : Component
    {
        OnTargetExit(target as TTarget);
        Target = null;

        if (DeSpawnMode == ItemDeSpawnMode.Exit)
        {
            gameObject.SetActive(false);
        }
    }

    public abstract void OnTargetEnter(TTarget target);
    public abstract void OnTargetExit(TTarget target);
}
