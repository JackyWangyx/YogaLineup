using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemWall : ItemBase<Player>
{
    public GirlFollow Girl;
    [Range(0f, 1f)]
    public float AnimationScale;

    public override void Init()
    {
        base.Init();
        Girl.Animator.speed = 0;
        UpdateAnimation();
    }

    public override void OnTargetEffect(Player target)
    {

    }

    [Button("Test")]
    public void UpdateAnimation()
    {
        Girl.Animator.speed = 0;
        Girl.Animator.Play("Idle", 0, AnimationScale);
    }
}
