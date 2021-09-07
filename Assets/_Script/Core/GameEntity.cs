using System.Collections;
using System.Collections.Generic;
using Aya.Events;
using UnityEngine;

public abstract class GameEntity : MonoListener
{

}

public abstract class GameEntity<T> : GameEntity where T : GameEntity<T>
{
    public static T Ins { get; protected set; }

    protected override void Awake()
    {
        base.Awake();
        Ins = this as T;
    }
}