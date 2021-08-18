using System;
using UnityEngine;

namespace Aya.TweenPro
{
    [Tweener("Transform Scale", "Transform")]
    [Serializable]
    public class TweenScale : TweenValueVector3<Transform>
    {
        public override Vector3 Value
        {
            get => Target.localScale;
            set => Target.localScale = value;
        }
    }

    #region Extension

    public static partial class UTween
    {
        public static TweenScale Scale(Transform transform, Vector3 to, float duration, SpaceMode spaceMode = SpaceMode.World)
        {
            var tweener = Play<TweenScale, Transform, Vector3>(transform, to, duration)
                .SetSpace(spaceMode)
                .SetCurrent2From() as TweenScale;
            return tweener;
        }

        public static TweenScale Scale(Transform transform, Vector3 from, Vector3 to, float duration, SpaceMode spaceMode = SpaceMode.World)
        {
            var tweener = Play<TweenScale, Transform, Vector3>(transform, from, to, duration)
                .SetSpace(spaceMode);
            return tweener;
        }
    }

    public static partial class TransformExtension
    {
        public static TweenScale TweenScale(this Transform transform, Vector3 to, float duration, SpaceMode spaceMode = SpaceMode.World)
        {
            var tweener = UTween.Scale(transform, to, duration, spaceMode);
            return tweener;
        }

        public static TweenScale TweenScale(this Transform transform, Vector3 from, Vector3 to, float duration, SpaceMode spaceMode = SpaceMode.World)
        {
            var tweener = UTween.Scale(transform, from, to, duration, spaceMode);
            return tweener;
        }
    }

    #endregion
}