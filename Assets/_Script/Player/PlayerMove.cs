using Aya.Extension;
using Sirenix.OdinInspector;

public class PlayerMove : PlayerBase
{
    public bool KeepUp;
    public float RunSpeed;
    public float RotateSpeed;
    public float TurnSpeed;
    public float TurnLerpSpeed;

    public PathFollower PathFollower { get; set; }

    public override void InitComponent()
    {
        PathFollower.Init(Self);
    }

    public override void CacheComponent()
    {
        base.CacheComponent();
        PathFollower = gameObject.GetOrAddComponent<PathFollower>();
    }

    public void EnableMove()
    {
        State.EnableRun = true;
        State.EnableInput = true;
        Play("Run");
    }

    public void DisableMove()
    {
        State.EnableRun = false;
        State.EnableInput = false;
        Play("Idle");
    }
}
