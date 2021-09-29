using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFollower : GameEntity
{
    public float Length { get; set; }
    public int CurrentBlockIndex { get; set; }
    public new LevelBlock CurrentBlock { get; set; }
    public bool Finish { get; set; }

    public float Distance { get; set; }
    public float Factor { get; set; }

    public float BlockDistance { get; set; }
    public float BlockFactor { get; set; }

    public void Init()
    {
        Finish = false;
        Distance = 0f;
        Factor = 0f;
        Length = 0f;
        foreach (var levelBlock in CurrentLevel.BlockList)
        {
            Length += levelBlock.Length;
        }

        EnterBlock(0);
    }

    public Vector3 Move(float distance)
    {
        if (Finish) return CurrentPath.GetPositionByFactor(1f);
        Vector3 result;

        while (true)
        {
            var overDistance = 0f;
            var finish = false;
            (finish, result, overDistance) = CurrentPath.GetPositionByDistance(distance);
            if (finish)
            {
                var enterResult = EnterBlock(CurrentBlockIndex + 1, overDistance);
                if (!enterResult)
                {
                    Finish = true;
                    break;
                }

                distance = 0f;
            }
            else
            {
                break;
            }
        }

        return result;
    }

    public bool EnterBlock(int index, float initDistance = 0f)
    {
        if (index >= CurrentLevel.BlockInsList.Count)
        {
            Finish = true;
            return false;
        }

        CurrentBlockIndex = index;
        CurrentBlock = CurrentLevel.BlockInsList[index];
        EnterPath(initDistance);
        Player.State.TurnRange = CurrentPath.TurnRange;

        return true;
    }

    public virtual void EnterPath(float initMoveDistance = 0f)
    {
        BlockDistance = 0f;
        BlockFactor = 0f;
        Move(initMoveDistance);
    }
}
