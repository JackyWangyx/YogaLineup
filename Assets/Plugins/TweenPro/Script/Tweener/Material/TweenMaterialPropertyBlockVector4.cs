using System;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Aya.TweenPro
{
    [Tweener("Material Vector4", "Material", "Material Icon")]
    [Serializable]
    public partial class TweenMaterialPropertyBlockVector4 : TweenValueVector4<Renderer>
    {
        public TweenMaterialData MaterialData;

        public override Vector4 Value
        {
            get
            {
                MaterialData.Cache(Target);
                return MaterialData.GetVector();
            }
            set
            {
                MaterialData.Cache(Target);
                MaterialData.SetVector(value);
            }
        }

        public override void Reset()
        {
            base.Reset();
            MaterialData.Reset();
        }
    }

#if UNITY_EDITOR

    public partial class TweenMaterialPropertyBlockVector4 : TweenValueVector4<Renderer>
    {
        public override void InitEditor(int index, TweenData data, SerializedProperty tweenerProperty)
        {
            base.InitEditor(index, data, tweenerProperty);
            MaterialData.InitEditor(this, tweenerProperty);
        }

        public override void DrawTarget()
        {
            base.DrawTarget();
            MaterialData.DrawMaterialProperty(ShaderUtil.ShaderPropertyType.Vector);
        }
    }

#endif

    #region Extension

    public partial class TweenMaterialPropertyBlockVector4 : TweenValueVector4<Renderer>
    {
        public TweenMaterialPropertyBlockVector4 SetMaterialMode(TweenMaterialMode materialMode)
        {
            MaterialData.Mode = materialMode;
            return this;
        }

        public TweenMaterialPropertyBlockVector4 SetMaterialIndex(int materialIndex)
        {
            MaterialData.Index = materialIndex;
            return this;
        }

        public TweenMaterialPropertyBlockVector4 SetPropertyName(string propertyName)
        {
            MaterialData.Property = propertyName;
            return this;
        }
    }

    public static partial class UTween
    {
        public static TweenMaterialPropertyBlockVector4 Vector4(Renderer renderer, string propertyName, Vector4 to, float duration)
        {
            var tweener = Vector4(renderer, 0, propertyName, to, duration);
            return tweener;
        }

        public static TweenMaterialPropertyBlockVector4 Vector4(Renderer renderer, int materialIndex, string propertyName, Vector4 to, float duration)
        {
            var tweener = Create<TweenMaterialPropertyBlockVector4>()
                .SetTarget(renderer)
                .SetMaterialIndex(materialIndex)
                .SetPropertyName(propertyName)
                .SetCurrent2From()
                .SetTo(to)
                .SetDuration(duration)
                .Play() as TweenMaterialPropertyBlockVector4;
            return tweener;
        }

        public static TweenMaterialPropertyBlockVector4 Vector4(Renderer renderer, string propertyName, Vector4 from, Vector4 to, float duration)
        {
            var tweener = Vector4(renderer, 0, propertyName, from, to, duration);
            return tweener;
        }

        public static TweenMaterialPropertyBlockVector4 Vector4(Renderer renderer, int materialIndex, string propertyName, Vector4 from, Vector4 to, float duration)
        {
            var tweener = Create<TweenMaterialPropertyBlockVector4>()
                .SetTarget(renderer)
                .SetMaterialIndex(materialIndex)
                .SetPropertyName(propertyName)
                .SetFrom(from)
                .SetTo(to)
                .SetDuration(duration)
                .Play() as TweenMaterialPropertyBlockVector4;
            return tweener;
        }
    }

    public static partial class RendererExtension
    {
        public static TweenMaterialPropertyBlockVector4 TweenVector4(this Renderer renderer, string propertyName, Vector4 to, float duration)
        {
            var tweener = UTween.Vector4(renderer, propertyName, to, duration);
            return tweener;
        }

        public static TweenMaterialPropertyBlockVector4 TweenVector4(this Renderer renderer, int materialIndex, string propertyName, Vector4 to, float duration)
        {
            var tweener = UTween.Vector4(renderer, materialIndex, propertyName, to, duration);
            return tweener;
        }

        public static TweenMaterialPropertyBlockVector4 TweenVector4(this Renderer renderer, string propertyName, Vector4 from, Vector4 to, float duration)
        {
            var tweener = UTween.Vector4(renderer, propertyName, from, to, duration);
            return tweener;
        }

        public static TweenMaterialPropertyBlockVector4 TweenVector4(this Renderer renderer, int materialIndex, string propertyName, Vector4 from, Vector4 to, float duration)
        {
            var tweener = UTween.Vector4(renderer, materialIndex, propertyName, from, to, duration);
            return tweener;
        }
    }

    #endregion
}
