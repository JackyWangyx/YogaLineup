using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStart : GamePhaseHandler
{
    public override PhaseType Type => PhaseType.Start;

    public override void Enter(params object[] args)
    {
        Camera.Switch("Game");
        UI.Show<UIGame>();
        Game.Enter<GamePlay>();
        Dispatch(GameEvent.Start);
    }

    public override void UpdateImpl()
    {

    }

    public override void Exit()
    {

    }
}
