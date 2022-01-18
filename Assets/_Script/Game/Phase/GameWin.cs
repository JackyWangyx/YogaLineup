using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameWin : GamePhaseHandler
{
    public override PhaseType Type => PhaseType.Win;

    public override void Enter(params object[] args)
    {
        Camera.Switch("Finish");
        UI.Show<UIWin>();
        Dispatch(GameEvent.Win);
    }

    public override void UpdateImpl()
    {

    }

    public override void Exit()
    {

    }
}
