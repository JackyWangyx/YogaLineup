using Aya.Maths;
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

    public (Vector3, float) GetNearestPos(Vector3 position)
    {
        var (result, startFactor, endFactor) = GetCenterPos(position, 0F, 1F, 0.05F);
        var pos = GetPosition(startFactor);
        return (pos, startFactor);
    }

    private (bool, float, float) GetCenterPos(Vector3 position, float start, float end, float limit)
    {
        var startPos = GetPosition(start);
        var endPos = GetPosition(end);

        var midPs = startPos + (endPos - startPos) / 2f;
        var midFactor = start + (end - start) / 2f;
        var startDis = MathUtil.SqrDistance(startPos, position);
        var endDis = MathUtil.SqrDistance(endPos, position);

        if (startDis < limit || endDis < limit)
        {
            return (true, start, end);
        }

        if (startDis < endDis)
        {
            return GetCenterPos(midPs, start, midFactor, limit);
        }
        else
        {
            return GetCenterPos(midPs, midFactor, end, limit);
        }
    }
}