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
        Play("Win");
        State.EnableRun = false;
        State.EnableInput = false;
    }

    public void Lose()
    {
        Play("Lose");
        State.RestoreSave();
        State.EnableRun = false;
        State.EnableInput = false;
    }

    public void Die()
    {
        Play("Lose");
        State.EnableRun = false;
        State.EnableInput = false;
    }
}
