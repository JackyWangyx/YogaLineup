using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UILose : UIBase<UILose>
{
    public void Retry()
    {
        Level.LevelStart();
    }
}
