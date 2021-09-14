using UnityEngine;

public abstract class BuffBase
{
    public Player Target;
    public float Duration;
    public float Timer;
    public float[] Args;

    public bool Active { get; set; }

    public virtual void Start(float duration, params float[] args)
    {
        Duration = duration;
        Timer = 0f;
        Args = args;
        Active = true;
        StartImpl();
    }

    public abstract void StartImpl();

    public virtual void Update(float deltaTime)
    {
        Timer += Time.deltaTime;
        if (Timer >= Duration) End();
        else UpdateImpl();
    }

    public abstract void UpdateImpl();

    public virtual void End()
    {
        Active = false;
        EndImpl();
    }

    public abstract void EndImpl();
}