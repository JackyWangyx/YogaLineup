using System;
using Aya.Extension;
using Aya.Physical;
using Sirenix.OdinInspector;
using UnityEngine;

public class Player : GameEntity
{
    [FoldoutGroup("Trans")] public Transform RenderTrans;

    [FoldoutGroup("Param")] public bool KeepUp;
    [FoldoutGroup("Param")] public float RunSpeed;
    [FoldoutGroup("Param")] public float RotateSpeed;
    [FoldoutGroup("Param")] public float TurnSpeed;
    [FoldoutGroup("Param")] public float TurnLerpSpeed;

    public bool IsPlayer => Player == this;
    public PathFollower PathFollower { get; set; }
    public PlayerState State { get; set; }

    public PlayerData Data { get; set; }
    public BuffManager Buff { get; set; } = new BuffManager();
    public Animator Animator { get; set; }
    public Rigidbody Rigidbody { get; set; }
    public GameObject RenderInstance { get; set; }

    public string CurrentClip { get; set; }

    protected override void Awake()
    {
        base.Awake();
        State = GetComponent<PlayerState>();

        CacheComponent();
    }

    #region Animator
    
    private string _lastAnimationClipName;
    public void Play(string animationClipName)
    {
        CurrentClip = animationClipName;
        if (Animator == null) Animator = GetComponentInChildren<Animator>(true);
        if (Animator != null)
        {
            if (Animator.CheckParameterExist(animationClipName, AnimatorControllerParameterType.Bool))
            {
                if (!string.IsNullOrEmpty(_lastAnimationClipName))
                {
                    Animator.SetBool(_lastAnimationClipName, false);
                }

                Animator.SetBool(animationClipName, true);
            }
            else
            {
                Animator.Play(animationClipName);
            }

            _lastAnimationClipName = animationClipName;
        }
    } 

    #endregion

    public void Init()
    {
        State.Init(this);
        Buff.Init(this);
        PathFollower.Init(this);
        RefreshRender(State.Point);
        Play("Idle");

        Trans.position = Vector3.zero;
        Trans.forward = Vector3.forward;
    }

    public void CacheComponent()
    {
        PathFollower = gameObject.GetOrAddComponent<PathFollower>();
        Animator = GetComponentInChildren<Animator>();
        Rigidbody = GetComponent<Rigidbody>();
        Renderer = GetComponentInChildren<Renderer>();
    }

    public void ChangePoint(int diff)
    {
        State.PointChanged = true;
        State.Point += diff;
        if (State.Point < 0) State.Point = 0;
        RefreshRender(State.Point);
    }

    public void RefreshRender(int point)
    {
        var datas = PlayerSetting.Ins.PlayerDatas;
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

            var playerRendererPrefab = AvatarSetting.Ins.SelectedAvatarList[rank];
            RenderInstance = GamePool.Spawn(playerRendererPrefab, RenderTrans);

            this.ExecuteNextFrame(() =>
            {
                CacheComponent();
                Play(CurrentClip);

                if (data.ChangeFx != null && State.PointChanged)
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
            var nextPathPos = PathFollower.Move(RunSpeed * State.SpeedMultiply * deltaTime);
            var nextPos = nextPathPos;

            if (nextPos != transform.position)
            {
                if (!State.KeepDirection)
                {
                    var rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(nextPos - transform.position), deltaTime * RotateSpeed).eulerAngles;
                    if (KeepUp)
                    {
                        rotation.x = 0f;
                    }
                    
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
