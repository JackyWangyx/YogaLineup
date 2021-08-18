using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIWin : UIBase
{
    public void NextLevel()
    {
        GameManager.Ins.NextLevel();
    }
}
