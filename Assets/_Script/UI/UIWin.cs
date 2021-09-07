using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIWin : UIBase<UIWin>
{
    public void NextLevel()
    {
        GameManager.Ins.NextLevel();
    }
}
