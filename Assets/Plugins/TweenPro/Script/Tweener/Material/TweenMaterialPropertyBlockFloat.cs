using System;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Aya.TweenPro
{
    [Tweener("Material Float", "Material", "Material Icon")]
    [Serializable]
    public partial class TweenMaterialPropertyBlockFloat : TweenValueFloat<Renderer>
    {
        public TweenMaterialData MaterialData;

        public override float Value
        {
            get
            {
                MaterialData.Cache(Target);
                return MaterialData.GetFloat();
            }
            set
            {
                MaterialData.Cache(Target);
                MaterialData.SetFloat(value);
            }
        }

        public override void Reset()
        {
            base.Reset();
            MaterialData.Reset();
        }
    }

#if UNITY_EDITOR

    public partial class TweenMaterialPropertyBlockFloat : TweenValueFloat<Renderer>
    {
        public override void InitEditor(int index, TweenData data, SerializedProperty tweenerProperty)
        {
            base.InitEditor(index, data, tweenerProperty);
            MaterialData.InitEditor(this, tweenerProperty);
        }

        public override void DrawTarget()
        {
            base.DrawTarget();
            MaterialData.DrawMaterialProperty(ShaderUtil.ShaderPropertyType.Float);
        }
    }

#endif

    #region Extension

    public partial class TweenMaterialPropertyBlockFloat : TweenValueFloat<Renderer>
    {
        public TweenMaterialPropertyBlockFloat SetMaterialMode(TweenMaterialMode materialMode)
        {
            MaterialData.Mode = materialMode;
            return this;
        }

        public TweenMaterialPropertyBlockFloat SetMaterialIndex(int materialIndex)
        {
            MaterialData.Index = materialIndex;
            return this;
        }

        public TweenMaterialPropertyBlockFloat SetPropertyName(string propertyName)
        {
            MaterialData.Property = propertyName;
            return this;
        }
    }

    public static partial class UTween
    {
        public static TweenMaterialPropertyBlockFloat Float(Renderer renderer, string propertyName, float to, float duration)
        {
            var tweener = Float(renderer, 0, propertyName, to, duration);
            return tweener;
        }

        public static TweenMaterialPropertyBlockFloat Float(Renderer renderer, int materialIndex, string propertyName, float to, float duration)
        {
            var tweener = Create<TweenMaterialPropertyBlockFloat>()
                .SetTarget(renderer)
                .SetMaterialIndex(materialIndex)
                .SetPropertyName(propertyName)
                .SetCurrent2From()
                .SetTo(to)
                .SetDuration(duration)
                .Play() as TweenMaterialPropertyBlockFloat;
            return tweener;
        }

        public static TweenMaterialPropertyBlockFloat Float(Renderer renderer, string propertyName, float from, float to, float duration)
        {
            var tweener = Float(renderer, 0, propertyName, from, to, duration);
            return tweener;
        }

        public static TweenMaterialPropertyBlockFloat Float(Renderer renderer, int materialIndex, string propertyName, float from, float to, float duration)
        {
            var tweener = Create<TweenMaterialPropertyBlockFloat>()
                .SetTarget(renderer)
                .SetMaterialIndex(materialIndex)
                .SetPropertyName(propertyName)
                .SetFrom(from)
                .SetTo(to)
                .SetDuration(duration)
                .Play() as TweenMaterialPropertyBlockFloat;
            return tweener;
        }
    }

    public static partial class RendererExtension
    {
        public static TweenMaterialPropertyBlockFloat TweenFloat(this Renderer renderer, string propertyName, float to, float duration)
        {
            var tweener = UTween.Float(renderer, propertyName, to, duration);
            return tweener;
        }

        public static TweenMaterialPropertyBlockFloat TweenFloat(this Renderer renderer, int materialIndex, string propertyName, float to, float duration)
        {
            var tweener = UTween.Float(renderer, materialIndex, propertyName, to, duration);
            return tweener;
        }

        public static TweenMaterialPropertyBlockFloat TweenFloat(this Renderer renderer, string propertyName, float from, float to, float duration)
        {
            var tweener = UTween.Float(renderer, propertyName, from, to, duration);
            return tweener;
        }

        public static TweenMaterialPropertyBlockFloat TweenFloat(this Renderer renderer, int materialIndex, string propertyName, float from, float to, float duration)
        {
            var tweener = UTween.Float(renderer, materialIndex, propertyName, from, to, duration);
            return tweener;
        }
    }

    #endregion
}
