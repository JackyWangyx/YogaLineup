using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState
{
    // General State
    public int Point;
    public int Rank;
    public bool PointChanged;

    // Buff State
    public bool IsInvincible;
    public float SpeedMultiply;

    public void Init()
    {
        PointChanged = false;
        Point = 0;
        Rank = -1;

        IsInvincible = false;
        SpeedMultiply = 1f;
    }
}
