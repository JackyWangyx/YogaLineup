using Aya.Extension;
using Aya.Particle;
using Dreamteck.Splines;
using UnityEngine;

public class Player : GameEntity
{
    protected SplineComputer Path;
    public Transform RenderTrans;
    public float RunSpeed;
    public float RotateSpeed;
    public float TurnSpeed;

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
            if (AddPointFx != null) ParticleSpawner.Spawn(AddPointFx, RenderTrans, RenderTrans.position);

        }
        else
        {
            if (ReducePointFx != null) ParticleSpawner.Spawn(ReducePointFx, RenderTrans, RenderTrans.position);
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
        TurnRange = Level.TurnRange;
        Path = Level.Path;
        transform.position = Path.EvaluatePosition(0);
        transform.forward = Vector3.forward;
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
                Destroy(RenderInstance);
            }

            RenderInstance = Instantiate(data.Player, RenderTrans);
            RenderInstance.transform.ResetLocal();

            CacheComponent();
            Play(CurrentClip);

            if (data.ChangeFx != null)
            {
                ParticleSpawner.Spawn(data.ChangeFx, RenderTrans, RenderTrans.position);
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
            var p = Path.Project(transform.position);
            var nextPercent = Path.Travel(p.percent, RunSpeed * Time.deltaTime, direction: Spline.Direction.Forward);
            var nextPos = Path.EvaluatePosition(nextPercent);
            if (nextPos != transform.position)
            {
                transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(nextPos - transform.position), Time.deltaTime * RotateSpeed);
                transform.position = nextPos;
            }
        }

        var canInput = Game.Phase == GamePhase.Gaming && EnableInput;
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
                var x = _startX + offset.x * TurnSpeed / 200f;
                x = Mathf.Clamp(x, TurnRange.x, TurnRange.y);
                RenderTrans.SetLocalPositionX(x);
            }
        }
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
