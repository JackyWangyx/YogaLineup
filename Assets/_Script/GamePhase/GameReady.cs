using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameReady : GamePhaseHandler
{
    public override PhaseType Type => PhaseType.Ready;

    public override void Enter()
    {
        Camera.Switch("Ready");
        UI.Show<UIReady>();
        Dispatch(GameEvent.Ready);
    }

    public override void UpdateImpl()
    {

    }

    public override void Exit()
    {

    }
}
