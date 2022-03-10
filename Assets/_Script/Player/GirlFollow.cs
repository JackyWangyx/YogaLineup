using Aya.Extension;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GirlFollow : GameEntity
{
    //private Transform Target => Player.Render.RenderTrans;
    private Transform Target;
    private Vector3 TransPos;
    private bool IsStart;

    public void Init(float LengthForPlayer,Transform target)
    {
        TransPos = Vector3.zero;
        TransPos.z = LengthForPlayer;
        Target = target;
        IsStart = true;
    }

    public void Run()
    {
        if (!IsStart)
            return;

        //var speed = Player.Move.MoveSpeed * Player.State.SpeedMultiply * DeltaTime;
        var pos = Level.Level.GetPositionY(Player.Move.PathFollower.Distance + TransPos.z);
        TransPos.y = pos.y;
        if (Mathf.Abs(Target.localPosition.x - RendererTrans.localPosition.x) <= 0.05f)
        {
            TransPos.x = Target.localPosition.x;
        }
        else
        {
            //var lerpScale = Mathf.Clamp(0.9f - SlowPower, 0.1f, 0.9f);
            var posX = Mathf.Lerp(RendererTrans.localPosition.x, Target.localPosition.x, YoGaGirlSetting.Ins.GirlFlowScale);
            TransPos.x = posX;
        }
        RendererTrans.localPosition = TransPos;
        RendererTrans.SetPositionZ(pos.z);

        if (Player.State.EnableRun)
        {
            string yogaStr = Player.Control._yogaList[Player.Control._targetIndex];
            if (!string.IsNullOrEmpty(CurrentClip))
                Animator.ResetTrigger(CurrentClip);
            Animator.SetTrigger(yogaStr);
            CurrentClip = yogaStr;
            //Play(yogaStr);
        }
    }
}
