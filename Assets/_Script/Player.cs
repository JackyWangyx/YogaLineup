using System;
using Aya.Extension;
using UnityEngine;

public class Player : GameEntity
{
    public Transform RenderTrans;
    public float RunSpeed;
    public float RotateSpeed;
    public float TurnSpeed;
    public float TurnLerpSpeed;

    [Header("Fx")]
    public GameObject AddPointFx;
    public GameObject ReducePointFx;

    public PlayerData Data { get; set; }
    public int Point { get; set; }
    public int Rank { get; set; }
    public bool PointChanged { get; set; }

    public Animator Animator { get; set; }
    public Rigidbody Rigidbody { get; set; }
    public Renderer Renderer { get; set; }
    public GameObject RenderInstance { get; set; }

    public string CurrentClip { get; set; }
    public bool EnableRun { get; set; }
    public bool EnableInput { get; set; }
    public Vector2 TurnRange { get; set; }

    protected override void Awake()
    {
        base.Awake();
        CacheComponent();
    }

    public void ChangePoint(int diff)
    {
        PointChanged = true;
        Point += diff;
        if (Point < 0) Point = 0;
        RefreshRender(Point);

        if (diff > 0)
        {
            if (AddPointFx != null) SpawnFx(AddPointFx, RenderTrans);
        }
        else
        {
            if (ReducePointFx != null) SpawnFx(ReducePointFx, RenderTrans);
        }
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
        PointChanged = false;
        Point = 0;
        Rank = -1;
        RefreshRender(Point);

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

        if (Rank != rank)
        {
            Rank = rank;
            Data = data;

            if (RenderInstance != null)
            {
                GamePool.DeSpawn(RenderInstance);
                RenderInstance = null;
            }

            RenderInstance = GamePool.Spawn(data.Player, RenderTrans);

            CacheComponent();
            Play(CurrentClip);

            if (data.ChangeFx != null)
            {
                SpawnFx(data.ChangeFx, RenderTrans);
            }
        }
    }

    public void Start()
    {
        
    }

    public void StartRun()
    {
        EnableRun = true;
        EnableInput = true;
    }

    private bool _isMouseDown;
    private Vector3 _startMousePos;
    private float _startX;

    public void Update()
    {
        if (EnableRun)
        {
            var nextPos = Level.Move(RunSpeed * Time.deltaTime);
            if (nextPos != transform.position)
            {
                var rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(nextPos - transform.position), Time.deltaTime * RotateSpeed).eulerAngles;
                rotation.x = 0f;
                transform.eulerAngles = rotation;
                transform.position = nextPos;
            }
        }

        var canInput = Game.Phase == PhaseType.Gaming && EnableInput;
        var turnX = RenderTrans.localPosition.x;
        if (canInput)
        {
            if (Input.GetMouseButtonDown(0) || (!EnableInput && Input.GetMouseButton(0)))
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

        turnX = Mathf.Clamp(turnX, TurnRange.x, TurnRange.y);
        turnX = Mathf.Lerp(RenderTrans.localPosition.x, turnX, TurnLerpSpeed * Time.deltaTime);
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
        EnableRun = false;
        EnableInput = false;
    }

    public void Die()
    {
        Play("Lose");
        EnableRun = false;
        EnableInput = false;
    }
}
