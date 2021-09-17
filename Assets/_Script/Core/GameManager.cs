using System;
using System.Collections.Generic;
using Aya.Extension;
using Sirenix.OdinInspector;
using UnityEngine;

[Serializable]
public class PlayerData
{
    [TableColumnWidth(30)] public int Point;
    [TableColumnWidth(50)] public GameObject Player;
    [TableColumnWidth(50)] public GameObject ChangeFx;
    [TableColumnWidth(30)] public Color Color;
}

public class GameManager : GameEntity<GameManager>
{
    [FoldoutGroup("Player")] public new Player Player;
    [FoldoutGroup("Player"), TableList] public List<PlayerData> PlayerDatas;

    [FoldoutGroup("Misc")]
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
        Level.LevelStart();
    }

    public void Enter<T>() where T : GamePhaseHandler
    {
        Enter(typeof(T));
    }

    public void Enter(Type phaseType)
    {
        var nextPhase = PhaseTypeDic[phaseType];
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
        if (CurrentPhase == null) return;
        CurrentPhase.UpdateImpl();
    }

    [Button("Clear Save Data"), GUIColor(1f, 0.5f, 0.5f)]
    public void ClearSaveData()
    {
        PlayerPrefs.DeleteAll();
    }
}