using System.Collections;
using System.Collections.Generic;
using Aya.Extension;
using UnityEngine;

public enum GameResult
{
    None = -1,
    Win = 1,
    Lose = 2,
}

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
        
        var result = CheckGameResult();
        if (result == GameResult.Lose)
        {
            _isOver = true;
            Player.Die();
            this.ExecuteDelay(() =>
            {
                Game.Enter<GameLose>();
            }, GeneralSetting.Ins.LoseWaitDuration);
        }
    }

    public virtual GameResult CheckGameResult()
    {
        if (Player.State.Point == 0 && Player.State.PointChanged)
        {
            return GameResult.Lose;
        }

        return GameResult.None;
    }

    public override void Exit()
    {

    }
}
