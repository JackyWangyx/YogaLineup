using Aya.Extension;
using Aya.Particle;
using Aya.Physical;
using Dreamteck.Splines;
using Sirenix.OdinInspector;
using UnityEngine;

public class Player : MonoBehaviour
{
    protected SplineComputer Path;
    public Transform RenderTrans;
    public float RunSpeed;
    public float RotateSpeed;
    public float TurnSpeed;

    [Header("Fx")]
    public GameObject AddPointFx;
    public GameObject ReducePointFx;

    public GameManager Game => GameManager.Ins;
    public Level Level => Game.Level;

    public PlayerData Data { get; set; }
    public int Point { get; set; }
    public int Rank { get; set; }

    public Animator Animator { get; set; }
    public Rigidbody Rigidbody { get; set; }
    public Renderer Renderer { get; set; }
    public GameObject RenderInstance { get; set; }

    public string CurrentClip { get; set; }

    public bool EnableRun { get; set; }

    public bool EnableInput { get; set; }

    public void Awake()
    {
        CacheComponent();
    }

    public void ChangePoint(int diff)
    {
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
        Animator?.Play(animationName);
    }

    public void Init()
    {
        Point = 0;
        RefreshRender(Point);
        Play("Idle");
        Path = GameManager.Ins.Level.Path;
        transform.position = Path.EvaluatePosition(0);
        transform.forward = Vector3.forward;
    }

    public void RefreshRender(int point)
    {
        if (RenderInstance != null)
        {
            Destroy(RenderInstance);
        }

        var datas = GameManager.Ins.PlayerDatas;
        var rank = 0;
        var data = datas[0];
        Rank = 0;
        for (var i = datas.Count - 1; i >= 0; i--)
        {
            data = datas[i];
            if (point >= data.Point)
            {
                RenderInstance = Instantiate(data.Player, RenderTrans);
                RenderInstance.transform.ResetLocal();
                rank = i + 1;
                break;
            }
        }

        if (rank != Rank)
        {
            Rank = rank;
            Data = data;
            CacheComponent();
            Play(CurrentClip);

            if (Data.ChangeFx != null)
            {
                ParticleSpawner.Spawn(Data.ChangeFx, RenderTrans, RenderTrans.position);
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

        var canInput = GameManager.Ins.Phase == GamePhase.Gaming && EnableInput;
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
                var range = GameManager.Ins.Level.Width / 2f;
                x = Mathf.Clamp(x, -range, range);
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
