using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLose : GamePhaseHandler
{
    public override PhaseType Type => PhaseType.Lose;

    public override void Enter()
    {
        Camera.Switch("Finish");
        UI.Show<UILose>();
        Dispatch(GameEvent.Lose);
    }

    public override void UpdateImpl()
    {

    }

    public override void Exit()
    {

    }
}
