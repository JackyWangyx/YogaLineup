using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UILose : UIBase
{
    public void Retry()
    {
        GameManager.Ins.LevelStart();
    }
}
