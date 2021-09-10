using UnityEngine;

public abstract class LevelPath : GameEntity
{
    public virtual Vector3 StartPosition { get; set; }
    public virtual Vector3 EndPosition { get; set; }

    public virtual Vector3 StartForward { get; set; }
    public virtual Vector3 EndForward { get; set; }

    public float Length { get; set; }
    public Vector3 CurrentPosition { get; set; }

    public virtual void Init()
    {
        CurrentPosition = GetPosition(0f);
    }

    public abstract Vector3 GetPosition(float factor);

    public virtual void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(GetPosition(0f), GetPosition(1f));
    }

    public float MoveDistance { get; set; }

    public virtual void Enter(float initMoveDistance = 0f)
    {
        MoveDistance = 0f;
        Move(initMoveDistance);
    }

    public abstract (bool, Vector3, float) Move(float distance);
}