using System;

public class PlayerState : GameEntity
{
    public new Player Player { get; set; }

    // General State
    [NonSerialized] public int Point;
    [NonSerialized] public int Rank;
    [NonSerialized] public bool PointChanged;

    // Buff State
    [NonSerialized] public bool IsInvincible;
    [NonSerialized] public float SpeedMultiply;

    public void Init(Player player)
    {
        Player = player;

        PointChanged = false;
        Point = player.InitPoint;
        Rank = -1;

        IsInvincible = false;
        SpeedMultiply = 1f;
    }
}
