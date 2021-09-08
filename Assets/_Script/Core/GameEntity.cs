﻿using System;
using Aya.Events;
using Aya.Particle;
using Aya.Pool;
using UnityEngine;

public abstract class GameEntity : MonoListener
{
    public GameManager Game => GameManager.Ins;
    public LayerSetting Layer => LayerSetting.Ins;
    public CameraManager Camera => CameraManager.Ins;
    public UIController UI => UIController.Ins;
    public ConfigManager Config => ConfigManager.Ins;
    public SaveManager Save => SaveManager.Ins;

    public Level Level => Game.Level;
    public Player Player => Game.Player;


    public EntityPool GamePool => PoolManager.Ins["Game"];
    public EntityPool EffectPool => ParticleSpawner.EntityPool;

    #region Fx
    
    public void SpawnFx(GameObject fxPrefab)
    {
        SpawnFx(fxPrefab, transform);
    }

    public void SpawnFx(GameObject fxPrefab, Transform parent)
    {
        SpawnFx(fxPrefab, parent, parent.transform.position);
    }

    public void SpawnFx(GameObject fxPrefab, Transform parent, Vector3 position)
    {
        ParticleSpawner.Spawn(fxPrefab, parent, position);
    }

    #endregion

    #region Event

    public void Dispatch<T>(T eventType, params object[] args)
    {
        UEvent.Dispatch(eventType, args);
    }

    public void DispatchTo<T>(T eventType, object target, params object[] args)
    {
        UEvent.DispatchTo(eventType, target, args);
    }

    public void DispatchTo<T>(T eventType, Predicate<object> predicate, params object[] args)
    {
        UEvent.DispatchTo(eventType, predicate, args);
    }

    public static void DispatchGroup<T>(T eventType, object group, params object[] args)
    {
        UEvent.DispatchGroup(eventType, group, args);
    }

    #endregion
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