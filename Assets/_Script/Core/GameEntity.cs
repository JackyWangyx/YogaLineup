using System;
using Aya.Events;
using Aya.Extension;
using Aya.Particle;
using Aya.Pool;
using UnityEngine;

public abstract class GameEntity : MonoListener
{
    public RectTransform Rect { get; set; }
    public Transform RendererTrans { get; set; }
    public Renderer Renderer { get; set; }
    public Rigidbody Rigidbody { get; set; }

    public GameManager Game => GameManager.Ins;
    public LevelManager Level => LevelManager.Ins;
    public LayerSetting Layer => LayerSetting.Ins;
    public UIController UI => UIController.Ins;
    public UpgradeManager Upgrade => UpgradeManager.Ins;
    public SaveManager Save => SaveManager.Ins;

    public Level CurrentLevel => Level.Level;

    public Player Player => Game.Player;

    public EntityPool GamePool => PoolManager.Ins["Game"];
    public EntityPool UIPool => PoolManager.Ins["UI"];
    public EntityPool EffectPool => ParticleSpawner.EntityPool;

    public virtual float DeltaTime => Time.deltaTime * SelfScale;
    public virtual float SelfScale { get; set; }

    protected override void Awake()
    {
        base.Awake();
        SelfScale = 1f;
        Trans = transform;
        CacheComponent();

        if (RendererTrans == null)
        {
            RendererTrans = transform;
        }
    }

    public virtual void CacheComponent()
    {
        Rect = GetComponent<RectTransform>();
        Renderer = transform.GetComponentInChildren<Renderer>();
        Animator = GetComponentInChildren<Animator>();
        Rigidbody = GetComponent<Rigidbody>();
        RendererTrans = transform.FindInAllChildFuzzy(nameof(Renderer));
    }

    #region Transform

    public Transform Trans { get; set; }

    public Transform Parent
    {
        get => Trans.parent;
        set => Trans.parent = value;
    }

    public Vector3 Position
    {
        get => Trans.position;
        set => Trans.position = value;
    }

    public Vector3 LocalPosition
    {
        get => Trans.localPosition;
        set => Trans.localPosition = value;
    }

    public Vector3 EulerAngles
    {
        get => Trans.eulerAngles;
        set => Trans.eulerAngles = value;
    }

    public Vector3 LocalEulerAngles
    {
        get => Trans.localEulerAngles;
        set => Trans.localEulerAngles = value;
    }

    public Vector3 LocalScale
    {
        get => Trans.localScale;
        set => Trans.localScale = value;
    }


    #endregion

    #region Camera

    public CameraManager Camera => CameraManager.Ins;
    public Camera MainCamera => Camera.Camera;
    public Camera UICamera => UI.Camera;

    #endregion

    #region Animator

    public Animator Animator { get; set; }
    public string CurrentClip { get; set; }

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
        if (fxPrefab == null) return;
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

    #region Setting

    public TSetting GetSetting<TSetting>() where TSetting : SettingBase<TSetting>
    {
        return SettingBase<TSetting>.Load<TSetting>();
    }

    #endregion

    #region Try

    public void TryCatch(Action action, string message)
    {
        try
        {
            action();
        }
        catch (Exception exception)
        {
            Debug.LogError(message + "\n" + exception);
        }
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