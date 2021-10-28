using System;
using UnityEngine;

public class PlayerState : GameEntity
{
    public new Player Player { get; set; }

    // General State
    [NonSerialized] public int Point;
    [NonSerialized] public int Rank;
    [NonSerialized] public bool PointChanged;

    // Run State
    [NonSerialized] public bool EnableRun;
    [NonSerialized] public bool EnableInput;
    [NonSerialized] public bool KeepDirection;
    [NonSerialized] public bool LimitTurnRange;
    [NonSerialized] public Vector2 TurnRange;

    // Buff State
    [NonSerialized] public bool IsInvincible;
    [NonSerialized] public float SpeedMultiply;

    [NonSerialized] public float EndlessRewardRate;

    public void Init(Player player)
    {
        Player = player;

        PointChanged = false;
        KeepDirection = false;

        EnableRun = false;
        EnableInput = false;

        Point = PlayerSetting.Ins.InitPoint;
        Rank = -1;

        LimitTurnRange = false;
        IsInvincible = false;
        SpeedMultiply = 1f;

        EndlessRewardRate = 1f;

        CacheSave();
    }

    private int _cacheCoin;

    public void CacheSave()
    {
        _cacheCoin = Save.Coin;
    }

    public void RestoreSave()
    {
        Save.Coin.Value = _cacheCoin;
    }
}