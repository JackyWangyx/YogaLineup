using System.Collections;
using System.Collections.Generic;
using Aya.Extension;
using UnityEngine;

public class PlayerControlPath : PlayerControl
{

    private bool _isMouseDown;
    private Vector3 _startMousePos;
    private float _startX;

    public override void UpdateImpl(float deltaTime)
    {
        if (State.EnableRun)
        {
            var nextPathPos = Move.MovePath(Move.MoveSpeed * State.SpeedMultiply * deltaTime);
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

        var canInput = Game.GamePhase == GamePhaseType.Gaming && State.EnableInput;
        var turnX = Self.Render.RenderTrans.localPosition.x;
        if (canInput)
        {
            if (Input.GetMouseButtonDown(0) || (!_isMouseDown && Input.GetMouseButton(0)))
            {
                _isMouseDown = true;
                _startMousePos = Input.mousePosition;
                _startX = Self.Render.RenderTrans.localPosition.x;
            }

            if (Input.GetMouseButtonUp(0))
            {
                _isMouseDown = false;
            }

            if (_isMouseDown)
            {
                var offset = Input.mousePosition - _startMousePos;
                turnX = _startX + offset.x * Move.TurnSpeed / 200f;
            }
        }

        turnX = Mathf.Clamp(turnX, State.TurnRange.x, State.TurnRange.y);
        turnX = Mathf.Lerp(Self.Render.RenderTrans.localPosition.x, turnX, Move.TurnLerpSpeed * deltaTime);
        Self.Render.RenderTrans.SetLocalPositionX(turnX);
    }
}
