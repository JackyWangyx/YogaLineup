using System;
using System.Collections.Generic;
using Aya.Extension;
using Aya.Particle;
using UnityEngine;

[Serializable]
public class PlayerData
{
    public int Point;
    public GameObject Player;
    public GameObject ChangeFx;
    public Color Color;
}

public class GameManager : GameEntity<GameManager>
{
    [Header("Level")]
    public int LevelIndex = 1;
    public int LevelCount;

    public List<PlayerData> PlayerDatas;
    public new Level Level { get; set; }
    public new Player Player;

    [Header("Option")] 
    public Transform PhaseHandler;

    public PhaseType Phase { get; set; }
    public GamePhaseHandler CurrentPhase { get; set; }
    public Dictionary<PhaseType, GamePhaseHandler> PhaseDic { get; protected set; }
    public Dictionary<Type, GamePhaseHandler> PhaseTypeDic { get; protected set; }
    public List<GamePhaseHandler> PhaseList { get; protected set; }

    protected override void Awake()
    {
        base.Awake();
        Phase = PhaseType.None;
        CurrentPhase = null;

        PhaseList = PhaseHandler.GetComponents<GamePhaseHandler>().ToList();
        PhaseDic = PhaseList.ToDictionary(p => p.Type);
        PhaseTypeDic = PhaseList.ToDictionary(p => p.GetType());
    }

    public virtual void Start()
    {
        LevelStart();
    }

    public virtual void NextLevel()
    {
        LevelIndex++;
        if (LevelIndex > LevelCount) LevelIndex = 1;

        LevelStart();
    }

    public virtual void LevelStart()
    {
        UI.HideAll();
        GamePool.DeSpawnAll();
        EffectPool.DeSpawnAll();
        var index = LevelIndex;

        if (Level != null)
        {
            Level = null;
        }

        Level = GamePool.Spawn(Resources.Load<Level>("Level/Level_" + index.ToString("D2")));
        Level.Trans.SetParent(null);
        Level.Init();

        Enter<GameReady>();
    }

    public void Enter<T>() where T : GamePhaseHandler
    {
        var nextPhase = PhaseTypeDic[typeof(T)];
        Enter(nextPhase);
    }

    public void Enter(PhaseType phaseType)
    {
        var nextPhase = PhaseDic[phaseType];
        Enter(nextPhase);
    }

    public void Enter(GamePhaseHandler nextPhase)
    {
        if (CurrentPhase != null) CurrentPhase.Exit();
        if (CurrentPhase != null && CurrentPhase.Type == nextPhase.Type) return;
        Phase = nextPhase.Type;
        nextPhase.Enter();
        CurrentPhase = nextPhase;
    }

    public void Update()
    {
        CurrentPhase.UpdateImpl();
    }
}
