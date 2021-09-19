using System;
using Aya.Extension;
using Sirenix.OdinInspector;
using UnityEngine;

public class Player : GameEntity
{
    [FoldoutGroup("Trans")] public Transform RenderTrans;

    [FoldoutGroup("Param")] public int InitPoint;
    [FoldoutGroup("Param")] public float RunSpeed;
    [FoldoutGroup("Param")] public float RotateSpeed;
    [FoldoutGroup("Param")] public float TurnSpeed;
    [FoldoutGroup("Param")] public float TurnLerpSpeed;

    public PlayerState State { get; set; }

    public PlayerData Data { get; set; }
    public BuffManager Buff { get; set; } = new BuffManager();
    public Animator Animator { get; set; }
    public Rigidbody Rigidbody { get; set; }
    public Renderer Renderer { get; set; }
    public GameObject RenderInstance { get; set; }

    public string CurrentClip { get; set; }

    protected override void Awake()
    {
        base.Awake();
        State = GetComponent<PlayerState>();

        CacheComponent();
    }

    public void ChangePoint(int diff)
    {
        State.PointChanged = true;
        State.Point += diff;
        if (State.Point < 0) State.Point = 0;
        RefreshRender(State.Point);
    }

    public void CacheComponent()
    {
        Animator = GetComponentInChildren<Animator>();
        Rigidbody = GetComponent<Rigidbody>();
        Renderer = GetComponentInChildren<Renderer>();
    }

    public void Play(string animationName)
    {
        CurrentClip = animationName;
        if (Animator == null) Animator = GetComponentInChildren<Animator>(true);
        if (Animator != null)
        {
            Animator.Play(animationName);
        }
    }

    public void Init()
    {
        State.Init(this);
        Buff.Init(this);
        RefreshRender(State.Point);
        Play("Idle");
    }

    public void RefreshRender(int point)
    {
        var datas = Game.PlayerDatas;
        var rank = 0;
        var data = datas[0];
        for (var i = 0; i < datas.Count; i++)
        {
            if (point >= datas[i].Point)
            {
                data = datas[i];
                rank = i;
            }
        }

        if (State.Rank != rank)
        {
            State.Rank = rank;
            Data = data;

            if (RenderInstance != null)
            {
                GamePool.DeSpawn(RenderInstance);
                RenderInstance = null;
            }

            RenderInstance = GamePool.Spawn(data.Player, RenderTrans);

            this.ExecuteNextFrame(() =>
            {
                CacheComponent();
                Play(CurrentClip);

                if (data.ChangeFx != null)
                {
                    SpawnFx(data.ChangeFx, RenderTrans);
                }
            });
        }
    }

    public void Start()
    {
        
    }

    public void StartRun()
    {
        State.EnableRun = true;
        State.EnableInput = true;
    }

    private bool _isMouseDown;
    private Vector3 _startMousePos;
    private float _startX;

    public void Update()
    {
        var deltaTime = DeltaTime;
        if (Game.Phase != PhaseType.Gaming) return;
        Buff.Update(deltaTime);
        if (State.EnableRun)
        {
            var nextPos = CurrentLevel.Move(RunSpeed * State.SpeedMultiply * deltaTime);
            if (nextPos != transform.position)
            {
                if (!State.KeepDirection)
                {
                    var rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(nextPos - transform.position), deltaTime * RotateSpeed).eulerAngles;
                    rotation.x = 0f;
                    transform.eulerAngles = rotation;
                }

                transform.position = nextPos;
            }
        }

        var canInput = Game.Phase == PhaseType.Gaming && State.EnableInput;
        var turnX = RenderTrans.localPosition.x;
        if (canInput)
        {
            if (Input.GetMouseButtonDown(0) || (!State.EnableInput && Input.GetMouseButton(0)))
            {
                _isMouseDown = true;
                _startMousePos = Input.mousePosition;
                _startX = RenderTrans.localPosition.x;
            }

            if (Input.GetMouseButtonUp(0))
            {
                _isMouseDown = false;
            }

            if (_isMouseDown)
            {
                var offset = Input.mousePosition - _startMousePos;
                turnX = _startX + offset.x * TurnSpeed / 200f;
            }
        }

        turnX = Mathf.Clamp(turnX, State.TurnRange.x, State.TurnRange.y);
        turnX = Mathf.Lerp(RenderTrans.localPosition.x, turnX, TurnLerpSpeed * deltaTime);
        RenderTrans.SetLocalPositionX(turnX);
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

    public void Die()
    {
        Play("Lose");
        State.EnableRun = false;
        State.EnableInput = false;
    }
}
