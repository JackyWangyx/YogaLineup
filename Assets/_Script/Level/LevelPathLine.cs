using UnityEngine;

public class LevelPathLine : LevelPath
{
    public Transform Start;
    public Transform End;

    public override Vector3 StartPosition => GetPosition(0f);
    public override Vector3 EndPosition => GetPosition(1f);

    public override Vector3 StartForward => EndPosition - StartPosition;
    public override Vector3 EndForward => EndPosition - StartPosition;

    public override void Init()
    {
        base.Init();
        Length = Vector3.Distance(StartPosition, EndPosition);
    }

    public override Vector3 GetPosition(float factor)
    {
        if (Start == null || End == null) return Vector3.zero;
        var position = Vector3.Lerp(Start.position, End.position, factor);
        return position;
    }

    public override (bool, Vector3, float) Move(float distance)
    {
        var finish = false;
        var overDistance = 0f;

        MoveDistance += distance;
        if (MoveDistance >= Length)
        {
            finish = true;
            overDistance = Length - MoveDistance;
            MoveDistance = Length;
        }

        var factor = MoveDistance / Length;
        var result = GetPosition(factor);
        CurrentPosition = result;

        return (finish, result, overDistance);
    }
}