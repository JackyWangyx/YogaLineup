using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLose : GamePhaseHandler
{
    public override GamePhaseType Type => GamePhaseType.Lose;

    public override void Enter(params object[] args)
    {
        Camera.Switch("Finish");
        UI.ShowWindow<UILose>();
        Dispatch(GameEvent.Lose);
    }

    public override void UpdateImpl()
    {

    }

    public override void Exit()
    {

    }
}
