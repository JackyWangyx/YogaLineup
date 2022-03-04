using Aya.Extension;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GirlFollow : GameEntity
{
    //private Transform Target => Player.Render.RenderTrans;
    private Transform Target;
    private Vector3 TransPos;

    public void Init(float LengthForPlayer,Transform target)
    {
        TransPos = Vector3.zero;
        TransPos.z = LengthForPlayer;
        Target = target;
    }

    public void Run()
    {
        var speed = Player.Move.MoveSpeed * Player.State.SpeedMultiply * DeltaTime;
        TransPos.y = Level.Level.GetPositionY(Player.Move.PathFollower.Distance + speed + TransPos.z);
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

        if (Player.State.EnableRun)
        {
            string yogaStr = Player.Control._yogaList[Player.Control._targetIndex];
            Play(yogaStr);
        }
    }
}
