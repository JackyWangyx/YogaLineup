using System;

public class PlayerState : GameEntity
{
    // General State
    [NonSerialized] public int Point;
    [NonSerialized] public int Rank;
    [NonSerialized] public bool PointChanged;

    // Buff State
    [NonSerialized] public bool IsInvincible;
    [NonSerialized] public float SpeedMultiply;

    public void Init()
    {
        PointChanged = false;
        Point = 0;
        Rank = -1;

        IsInvincible = false;
        SpeedMultiply = 1f;
    }
}
