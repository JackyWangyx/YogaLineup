﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelBlock : GameEntity
{
    public float Width;
    public float HalfWidth => Width / 2f;
    public Vector2 TurnRange => new Vector2(-HalfWidth, HalfWidth);

    public LevelPath Path { get; set; }

    public Vector3 StartPosition => Path.StartPosition;
    public Vector3 EndPosition => Path.EndPosition;

    public Vector3 StartForward => Path.StartForward;
    public Vector3 EndForward => Path.EndForward;

    protected override void Awake()
    {
        base.Awake();
        Path = GetComponent<LevelPath>();
    }

    public void Init()
    {
        Path.Init();
    }

    public void OnDrawGizmos()
    {
        var length = 100f;
        Gizmos.color = Color.red;
        Gizmos.DrawLine(Vector3.left * length, Vector3.right * length);
        Gizmos.color = Color.green;
        Gizmos.DrawLine(Vector3.zero, Vector3.up * length);
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(Vector3.zero, Vector3.forward * length);
    }
}
