﻿using System.Collections.Generic;
using UnityEngine;
using Aya.Extension;

public class PathFollowerGirl : GameEntity
{
    #region Path

    public class Node
    {
        public Vector3 Pos;
        public float Distance;
    }

    public class Path
    {
        public List<Node> Paths { get; set; } = new List<Node>();
        public float Distance { get; set; } = 0f;
        public float MaxDistance { get; set; }

        public void Add(Vector3 pos)
        {
            var node = new Node()
            {
                Pos = pos
            };
            var lastNode = Paths.Last();
            if (lastNode != null)
            {
                var dis = Vector3.Distance(lastNode.Pos, node.Pos);
                node.Distance = dis;
                Distance += dis;
            }

            Paths.Add(node);

            while (Distance > MaxDistance)
            {
                var first = Paths.First();
                if (first == null) break;
                var dis = first.Distance;
                Distance -= dis;
                Paths.RemoveAt(0);
            }
        }

        public Vector3 GetPos(float followDistance)
        {
            var dis = 0f;
            for (var i = Paths.Count - 1; i >= 0; i--)
            {
                var node = Paths[i];
                if (dis + node.Distance >= followDistance)
                {
                    var current = Paths[i];
                    var next = Paths.Before(current);
                    if (next == null) return current.Pos;
                    dis += node.Distance;
                    var diff = dis - followDistance;
                    var factor = ((current.Distance - diff) / current.Distance);
                    var pos = Vector3.Lerp(current.Pos, next.Pos, factor);
                    return pos;
                }
                else
                {
                    dis += node.Distance;
                    continue;
                }
            }

            var firstNode = Paths.First();
            if (firstNode == null) return Vector3.zero;
            return firstNode.Pos;
        }

        public void Clear()
        {
            Paths.Clear();
            Distance = 0f;
        }
    }

    #endregion

    public Transform Target;
    public float KeepDistance;
    public float MaxDistance;

    public Path FollowPath { get; set; }

    public void Init()
    {
        FollowPath = new Path()
        {
            MaxDistance = MaxDistance
        };
    }

    public void LateUpdate()
    {
        if (Target == null) return;
        FollowPath.Add(Target.position);
        var lastPos = transform.position;
        var pos = FollowPath.GetPos(KeepDistance);
        transform.position = pos;
        if (pos == lastPos)
        {
            transform.forward = Target.forward;
        }
        else
        {
            transform.forward = Target.forward;
        }

        if (Player.State.EnableRun)
        {
            string yogaStr = Player.Control._yogaList[Player.Control._targetIndex];
            Play(yogaStr);
        }
    }
}