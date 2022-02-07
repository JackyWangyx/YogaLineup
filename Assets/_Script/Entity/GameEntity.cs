using System;
using System.Collections.Generic;
using System.Reflection;
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

        CacheSubPoolInstance();
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

    public Quaternion Rotation
    {
        get => Trans.rotation;
        set => Trans.rotation = value;
    }

    public Quaternion LocalRotation
    {
        get => Trans.localRotation;
        set => Trans.localRotation = value;
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

    public void InitAnimator()
    {
        if (Animator == null) return;
        foreach (var animatorControllerParameter in Animator.parameters)
        {
            if (animatorControllerParameter.type == AnimatorControllerParameterType.Bool)
            {
                Animator.SetBool(animatorControllerParameter.name, false);
            }
        }
    }

    public void Play(string animationClipName, bool immediately = false)
    {
        CurrentClip = animationClipName;
        if (Animator == null) Animator = GetComponentInChildren<Animator>(true);
        if (Animator != null)
        {
            if (!immediately && Animator.CheckParameterExist(animationClipName, AnimatorControllerParameterType.Bool))
            {
                if (!string.IsNullOrEmpty(_lastAnimationClipName))
                {
                    Animator.SetBool(_lastAnimationClipName, false);
                }

                Animator.SetBool(animationClipName, true);
            }
            else
            {
                if (Animator.CheckStateExist(animationClipName))
                {
                    Animator.Play(animationClipName);
                }
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
        return SettingBase<TSetting>.Load();
    }

    #endregion

    #region Sub Pool Instance

    public List<PropertyInfo> SubPoolInstanceList { get; set; } = new List<PropertyInfo>();

    public virtual void CacheSubPoolInstance()
    {
        SubPoolInstanceList.Clear();
        SubPoolInstanceList = this.GetPropertiesWithAttribute<SubPoolInstanceAttribute>();
    }

    public virtual void DeSpawnSubPoolInstance()
    {
        foreach (var propertyInfo in SubPoolInstanceList)
        {
            var instance = propertyInfo.GetValue(this);
            if (instance == null) continue;
            if (instance is GameObject go)
            {
                GamePool.DeSpawn(go);
            }
            else if (instance is MonoBehaviour behaviour)
            {
                GamePool.DeSpawn(behaviour.gameObject);
            }

            propertyInfo.SetValue(this, null);
        }
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

    #region MonoBehaviour

    protected override void OnDisable()
    {
        base.OnDisable();
        DeSpawnSubPoolInstance();
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