using System;
using UnityEngine;
using Random = UnityEngine.Random;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Aya.TweenPro
{
    [Tweener("Transform Shake", "Transform")]
    [Serializable]
    public partial class TweenShake : TweenValueFloat<Transform>
    {
        public TweenShakeData ShakeData;

        public override bool SupportIndependentAxis => true;
        public override bool SupportSetCurrentValue => false;
        public override bool SupportSpace => true;

        public override int AxisCount => 3;

        public override float Value { get; set; }

        public Vector3 ValuePosition
        {
            get => Space == SpaceMode.World ? Target.position : Target.localPosition;
            set
            {
                if (Space == SpaceMode.World)
                {
                    Target.position = value;
                }
                else
                {
                    Target.localPosition = value;
                }
            }
        }

        public Vector3 ValueRotation
        {
            get => Space == SpaceMode.World ? Target.eulerAngles : Target.localEulerAngles;
            set
            {
                if (Space == SpaceMode.World)
                {
                    Target.eulerAngles = value;
                }
                else
                {
                    Target.localEulerAngles = value;
                }
            }
        }

        public Vector3 ValueScale
        {
            get => Target.localScale;
            set => Target.localScale = value;
        }

        private Vector3 _offsetPosition;
        private Vector3 _offsetRotation;
        private Vector3 _offsetScale;

        private Vector3 _startPosition;
        private Vector3 _startRotation;
        private Vector3 _startScale;

        public override void RecordObject()
        {
            if (AxisX) _startPosition = ValuePosition;
            if (AxisY) _startRotation = ValueRotation;
            if (AxisZ) _startScale = ValueScale;

            if (ShakeData.Mode == ShakeMode.Random)
            {
                _offsetPosition = new Vector3(
                    Random.Range(-ShakeData.Position.x, ShakeData.Position.x),
                    Random.Range(-ShakeData.Position.y, ShakeData.Position.y),
                    Random.Range(-ShakeData.Position.z, ShakeData.Position.z));
                _offsetRotation = new Vector3(
                    Random.Range(-ShakeData.Rotation.x, ShakeData.Rotation.x),
                    Random.Range(-ShakeData.Rotation.y, ShakeData.Rotation.y),
                    Random.Range(-ShakeData.Rotation.z, ShakeData.Rotation.z));
                _offsetScale = new Vector3(
                    Random.Range(-ShakeData.Scale.x, ShakeData.Scale.x),
                    Random.Range(-ShakeData.Scale.y, ShakeData.Scale.y),
                    Random.Range(-ShakeData.Scale.z, ShakeData.Scale.z));
            }

            if (ShakeData.Mode == ShakeMode.Definite)
            {
                _offsetPosition = ShakeData.Position;
                _offsetRotation = ShakeData.Rotation;
                _offsetScale = ShakeData.Scale;
            }
        }

        public override void RestoreObject()
        {
            if (AxisX) ValuePosition = _startPosition;
            if (AxisY) ValueRotation = _startRotation;
            if (AxisZ) ValueScale = _startScale;
        }

        public override void Sample(float factor)
        {
            var step = 1f / ShakeData.Count;
            var power = 1f - factor;
            var curveFactor = (factor % step) / step;
            var currentFactor = ShakeData.Curve.Evaluate(curveFactor) * power;
            if (AxisX)
            {
                var position = Vector3.LerpUnclamped(_startPosition, _startPosition + _offsetPosition, currentFactor);
                ValuePosition = position;
            }

            if (AxisY)
            {
                var rotation = Vector3.LerpUnclamped(_startRotation, _startRotation + _offsetRotation, currentFactor);
                ValueRotation = rotation;
            }

            if (AxisZ)
            {
                var scale = Vector3.LerpUnclamped(_startScale, _startScale + _offsetScale, currentFactor);
                ValueScale = scale;
            }
        }

        public override void Reset()
        {
            base.Reset();
            ShakeData.Reset();
            AxisX = true;
            AxisY = false;
            AxisZ = false;
#if UNITY_EDITOR
            EnableIndependentAxis = true;
#endif
        }
    }

#if UNITY_EDITOR

    public partial class TweenShake : TweenValueFloat<Transform>
    {
        public override string AxisXName => "P";
        public override string AxisYName => "R";
        public override string AxisZName => "S";

        public override void InitEditor(int index, TweenData data, SerializedProperty tweenerProperty)
        {
            base.InitEditor(index, data, tweenerProperty);
            ShakeData.InitEditor(this, tweenerProperty);
        }

        public override void DrawIndependentAxis()
        {
        }

        public override void DrawFromToValue()
        {
            ShakeData.DrawShakeData();
            base.DrawFromToValue();
            From = Mathf.Clamp01(From);
            To = Mathf.Clamp01(To);
        }
    }

#endif
}
