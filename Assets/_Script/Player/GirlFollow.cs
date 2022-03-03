using Aya.Extension;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GirlFollow : GameEntity
{
    private Transform Target => Player.RendererTrans;

    public void Update()
    {
        var posX = Mathf.Lerp(RendererTrans.position.x, Target.position.x, 0.5f);
        //Debug.Log(RendererTrans.position.x);
        //Debug.Log(Target.position.x);
        RendererTrans.SetPositionX(posX);
    }
}
