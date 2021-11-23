using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePlay : GamePhaseHandler
{
    public override PhaseType Type => PhaseType.Gaming;

    public override void Enter()
    {

    }

    public override void UpdateImpl()
    {
        if (Player.State.Point == 0 && Player.State.PointChanged)
        {
            Game.Enter<GameLose>();
        }
    }

    public override void Exit()
    {

    }
}
