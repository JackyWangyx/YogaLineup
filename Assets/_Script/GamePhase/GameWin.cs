using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameWin : GamePhaseHandler
{
    public override PhaseType Type => PhaseType.Win;

    public override void Enter()
    {
        Camera.Switch("Finish");
        UI.Show<UIWin>();
        Player.Win();
        Dispatch(GameEvent.Win);
    }

    public override void UpdateImpl()
    {

    }

    public override void Exit()
    {

    }
}
