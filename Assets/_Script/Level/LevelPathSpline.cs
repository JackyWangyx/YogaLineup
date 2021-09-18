using Dreamteck.Splines;
using UnityEngine;

public class LevelPathSpline : LevelPath
{
    public SplineComputer Path { get; set; }

    public override Vector3 StartPosition => GetPosition(0f);
    public override Vector3 EndPosition => GetPosition(1f);
    public override Vector3 StartForward => GetForward(0f);
    public override Vector3 EndForward => GetForward(1f);

    public override void Init()
    {
        Path = GetComponent<SplineComputer>();
        Path.RebuildImmediate(true, true);
        Length = Path.CalculateLength();
    }

    public override Vector3 GetPosition(float factor)
    {
        if (Path == null) return Vector3.zero;
        var position = Path.Evaluate(factor).position;
        return position;
    }

    public virtual Vector3 GetForward(float factor)
    {
        if (Path == null) return Vector3.forward;
        var position = Path.Evaluate(factor).forward;
        return position;
    }

    public override void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        for (var i = 0; i < 100; i++)
        {
            Gizmos.DrawLine(GetPosition(i * 1f / 100f), GetPosition((i + 1) * 1f / 100f));
        }
    }

    private double _percent;

    public override void Enter(float initMoveDistance = 0)
    {
        _percent = 0f;
        base.Enter(initMoveDistance);
    }

    public override (bool, Vector3, float) Move(float distance)
    {
        var finish = false;
        var overDistance = 0f;

        _percent = Path.Travel(_percent, distance);
        var result = Path.EvaluatePosition(_percent);
        MoveDistance += distance;

        if (MoveDistance >= Length)
        {
            finish = true;
            overDistance = MoveDistance - Length;
            MoveDistance = Length;
        }

        CurrentPosition = result;

        return (finish, result, overDistance);
    }
}