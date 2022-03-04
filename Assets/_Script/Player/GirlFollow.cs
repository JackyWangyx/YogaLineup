using Aya.Extension;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GirlFollow : GameEntity
{
    //private Transform Target => Player.Render.RenderTrans;
    private Transform Target;
    private Vector3 TransPos;
    private float SlowPower;

    public void Init(float LengthForPlayer,float slowPower ,Transform target)
    {
        TransPos = Vector3.zero;
        TransPos.z = LengthForPlayer;
        SlowPower = slowPower;
        Target = target;
    }

    public void Update()
    {
        if (Mathf.Abs(Target.localPosition.x - RendererTrans.localPosition.x) <= 0.05f)
        {
            TransPos.x = Target.localPosition.x;
        }
        else
        {
            var lerpScale = Mathf.Clamp(0.9f - SlowPower, 0.1f, 0.9f);
            var posX = Mathf.Lerp(RendererTrans.localPosition.x, Target.localPosition.x, 0.5f);
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
