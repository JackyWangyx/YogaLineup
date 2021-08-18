using System;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Aya.TweenPro
{
    [Serializable]
    public abstract partial class TweenValueQuaternion<TTarget> : Tweener<TTarget, Quaternion>
        where TTarget : UnityEngine.Object
    {
        public override bool SupportIndependentAxis => true;
        public override int AxisCount => 4;

        public override void Sample(float factor)
        {
            var from = FromGetter();
            var to = ToGetter();
            var result = ValueGetter();
            var temp = Quaternion.LerpUnclamped(from, to, factor);
            if (AxisX) result.x = temp.x;
            if (AxisY) result.y = temp.y;
            if (AxisZ) result.z = temp.z;
            if (AxisW) result.w = temp.w;
            ValueSetter(result);
            OnUpdate?.Invoke(result);
        }

        public override void Reset()
        {
            base.Reset();
            From = Quaternion.identity;
            To = Quaternion.identity;
        }
    }

#if UNITY_EDITOR

    public abstract partial class TweenValueQuaternion<TTarget> : Tweener<TTarget, Quaternion>
        where TTarget : UnityEngine.Object
    {
        public override void DrawFromToValue()
        {
            GUIUtil.DrawQuaternionProperty(HoldStartProperty, FromProperty, nameof(From),
                AxisXName, AxisYName, AxisZName, AxisWName,
                AxisX, AxisY, AxisZ, AxisW);
            GUIUtil.DrawQuaternionProperty(HoldEndProperty, ToProperty, nameof(To),
                AxisXName, AxisYName, AxisZName, AxisWName,
                AxisX, AxisY, AxisZ, AxisW);
        }
    }
#endif
}
