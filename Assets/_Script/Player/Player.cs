using Aya.Extension;
using Sirenix.OdinInspector;
using UnityEngine;

public class Player : PlayerBase
{
    public PlayerData Data { get; set; }
    public bool IsPlayer => Player == this;

    protected override void Awake()
    {
        base.Awake();
    }

    public void Init()
    {
        InitAllComponent();

        Play("Idle");
        Trans.position = Vector3.zero;
        Trans.forward = Vector3.forward;
    }

    

    public void Start()
    {
        
    }

    public void FixedUpdate()
    {


    }

    public void LateUpdate()
    {

    }

    public void Win()
    {
        if (Game.Phase != PhaseType.Gaming) return;
        Play("Win");
        State.EnableRun = false;
        State.EnableInput = false;

        if (IsPlayer) Game.Enter<GameWin>();
    }

    public void Lose()
    {
        if (Game.Phase != PhaseType.Gaming) return;
        Play("Lose");
        State.RestoreSave();
        State.EnableRun = false;
        State.EnableInput = false;
        if (IsPlayer) Game.Enter<GameLose>();
    }

    public void Die()
    {
        if (Game.Phase != PhaseType.Gaming) return;
        Play("Lose");
        State.EnableRun = false;
        State.EnableInput = false;
        if (IsPlayer) Game.Enter<GameLose>();
    }
}
