using Aya.Events;
using Aya.Particle;
using Aya.Pool;

public abstract class GameEntity : MonoListener
{
    public GameManager Game => GameManager.Ins;
    public LayerSetting Layer => LayerSetting.Ins;
    public CameraManager Camera => CameraManager.Ins;
    public UIController UI => UIController.Ins;
    public ConfigManager Config => ConfigManager.Ins;
    public SaveManager Save => SaveManager.Ins;

    public Level Level => Game.Level;
    public Player Player => Game.Player;


    public EntityPool GamePool => PoolManager.Ins["Game"];
    public EntityPool EffectPool => ParticleSpawner.EntityPool;
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