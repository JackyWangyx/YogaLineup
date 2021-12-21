﻿using Aya.Extension;
using UnityEngine;

public class PlayerInput : PlayerBase
{
    protected override void Awake()
    {
        base.Awake();

        Self = GetComponent<Player>();
    }

    private bool _isMouseDown;
    private Vector3 _startMousePos;
    private float _startX;

    public void Update()
    {
        var deltaTime = DeltaTime;
        if (Game.Phase != PhaseType.Gaming) return;

        if (State.EnableRun)
        {
            var nextPathPos = Move.PathFollower.Move(Move.RunSpeed * State.SpeedMultiply * deltaTime);
            var nextPos = nextPathPos;

            if (nextPos != transform.position)
            {
                if (!State.KeepDirection)
                {
                    var rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(nextPos - transform.position), deltaTime * Move.RotateSpeed).eulerAngles;
                    if (Move.KeepUp)
                    {
                        rotation.x = 0f;
                    }

                    transform.eulerAngles = rotation;
                }

                transform.position = nextPos;
            }
        }

        var canInput = Game.Phase == PhaseType.Gaming && State.EnableInput;
        var turnX = Self.Render.RenderTrans.localPosition.x;
        if (canInput)
        {
            if (UnityEngine.Input.GetMouseButtonDown(0) || (!_isMouseDown && UnityEngine.Input.GetMouseButton(0)))
            {
                _isMouseDown = true;
                _startMousePos = UnityEngine.Input.mousePosition;
                _startX = Self.Render.RenderTrans.localPosition.x;
            }

            if (UnityEngine.Input.GetMouseButtonUp(0))
            {
                _isMouseDown = false;
            }

            if (_isMouseDown)
            {
                var offset = UnityEngine.Input.mousePosition - _startMousePos;
                turnX = _startX + offset.x * Move.TurnSpeed / 200f;
            }
        }

        turnX = Mathf.Clamp(turnX, State.TurnRange.x, State.TurnRange.y);
        turnX = Mathf.Lerp(Self.Render.RenderTrans.localPosition.x, turnX, Move.TurnLerpSpeed * deltaTime);
        Self.Render.RenderTrans.SetLocalPositionX(turnX);
    }
}
