using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIGame : UIBase<UIGame>
{
    public void Update()
    {
       
    }

    public void Retry()
    {
        GameManager.Ins.LevelStart();
    }
}
