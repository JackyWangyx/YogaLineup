using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemWall : ItemBreakable
{
    public GirlFollow Girl;
    [Range(0f, 1f)]
    public float AnimationScale;
    public float StopTime;
    public SkinnedMeshRenderer GirlShader;
    private bool CanGo;

    public override void Init()
    {
        base.Init();
        CanGo = false;
        Girl.Animator.speed = 0;
        UpdateAnimation();
    }

    public override void OnTargetEffect(Player target)
    {
        if (target.transform.position.z > transform.position.z)
            return;

        if (!CanGo)
        {
            target.Render.AddRender(null, Player.Data.Size, -1);
            Player.Move.Stop(StopTime);
        }
    }

    public void Update()
    {
        var distance = Mathf.Abs(Player.Control.AnimationScale - AnimationScale);
        if (distance <= 0.05f)
        {
            CanGo = true;
            GirlShader.materials[0].SetColor("_OutlineColor", Color.green);
        }
        else
        {
            CanGo = false;
            GirlShader.materials[0].SetColor("_OutlineColor", Color.red);
        }
    }

    [Button("Test")]
    public void UpdateAnimation()
    {
        Girl.Animator.speed = 0;
        Girl.Animator.Play("Idle", 0, AnimationScale);
    }
}
