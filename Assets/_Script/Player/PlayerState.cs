using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState : PlayerBase
{
    [NonSerialized] public new int Index;
    [NonSerialized] public new bool IsPlayer;
    /// <summary>
    /// 包括本体的少女列表(本体为0)
    /// </summary>
    [NonSerialized] public List<Animator> YogaGirlList = new List<Animator>();

    // Health
    [NonSerialized] public int Hp;

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

    private int _cacheCoin;

    public override void InitComponent()
    {
        Hp = Health.MaxHp;

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

    //public void ChangePoint(int diff)
    //{
    //    State.PointChanged = true;
    //    State.Point += diff;
    //    if (State.Point < 0) State.Point = 0;
    //    Render.RefreshRender(State.Point);
    //}

    public void ChangePoint(int diff)
    {
        State.PointChanged = true;
        State.Point += diff;
        if (State.Point < 0) State.Point = 0;
        Render.RefreshRender(State.Point);
    }

    public void CacheSave()
    {
        _cacheCoin = Save.Coin;
    }

    public void RestoreSave()
    {
        Save.Coin.Value = _cacheCoin;
    }
}