using Aya.Extension;
using Sirenix.OdinInspector;
using UnityEngine;

public class PlayerMove : PlayerBase
{
    public bool KeepUp;
    public float MoveSpeed;
    public float RotateSpeed;
    public float TurnSpeed;
    public float TurnLerpSpeed;

    public override void InitComponent()
    {
        PathFollower.Init(Self);
    }

    public override void CacheComponent()
    {
        base.CacheComponent();
        PathFollower = gameObject.GetOrAddComponent<PathFollower>();
    }

    #region Path

    public PathFollower PathFollower { get; set; }
    public LevelBlock CurrentBlock => PathFollower.CurrentBlock;
    public LevelPath CurrentPath => PathFollower.CurrentPath;

    public Vector3 MovePath(float distance)
    {
        var position = PathFollower.Move(distance);
        return position;
    }

    public void EnterBlock(int index)
    {
        PathFollower.EnterBlock(index);
    }

    public void EnterPath(int index)
    {
        PathFollower.EnterPath(index);
    }

    public void SwitchPath(int blockIndex, int pathIndex)
    {
        PathFollower.SwitchPath(blockIndex, pathIndex);
    }

    #endregion

    #region lOmnidirectional

    public Vector3 MoveDirection(Vector3 direction, float distance)
    {
        var currentPosition = Position;
        currentPosition += direction * distance;
        Position = currentPosition;
        Forward = Vector3.Lerp(Forward, direction, RotateSpeed * DeltaTime);
        return currentPosition;
    }

    #endregion

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