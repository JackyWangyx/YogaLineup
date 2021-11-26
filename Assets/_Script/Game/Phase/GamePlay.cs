using System.Collections;
using System.Collections.Generic;
using Aya.Extension;
using UnityEngine;

public class GamePlay : GamePhaseHandler
{
    public override PhaseType Type => PhaseType.Gaming;
    private bool _isOver;

    public override void Enter()
    {
        _isOver = false;
    }

    public override void UpdateImpl()
    {
        if (_isOver) return;
        _isOver = true;
        Player.Die();
        this.ExecuteDelay(() =>
        {
            Game.Enter<GameLose>();
        }, GeneralSetting.Ins.LoseWaitDuration);
    }

    public override void Exit()
    {

    }
}
