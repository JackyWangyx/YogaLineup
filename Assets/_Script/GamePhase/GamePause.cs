﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePause : GamePhaseHandler
{
    public override PhaseType Type => PhaseType.Pause;

    public override void Enter()
    {
        Dispatch(GameEvent.Pause);
    }

    public override void UpdateImpl()
    {

    }

    public override void Exit()
    {

    }
}