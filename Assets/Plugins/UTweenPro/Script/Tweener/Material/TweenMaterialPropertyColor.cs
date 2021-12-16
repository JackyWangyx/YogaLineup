using System;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Aya.TweenPro
{
    [Tweener("Material Color", "Material", "Material Icon")]
    [Serializable]
    public partial class TweenMaterialPropertyColor : TweenValueColor<Renderer>
    {
        public TweenMaterialData MaterialData = new TweenMaterialData();

        public override Color Value
        {
            get
            {
                MaterialData.Cache(Target);
                return MaterialData.GetColor();
            }
            set
            {
                MaterialData.Cache(Target);
                MaterialData.SetColor(value);
            }
        }

        public override void Reset()
        {
            base.Reset();
            MaterialData.Reset();
        }
    }

#if UNITY_EDITOR

    public partial class TweenMaterialPropertyColor : TweenValueColor<Renderer>
    {
        public override void InitEditor(int index, TweenData data, SerializedProperty tweenerProperty)
        {
            base.InitEditor(index, data, tweenerProperty);
            MaterialData.InitEditor(this, tweenerProperty);
        }

        public override void DrawTarget()
        {
            base.DrawTarget();
            MaterialData.DrawMaterialProperty(ShaderUtil.ShaderPropertyType.Color);
        }
    }

#endif

    #region Extension

    public partial class TweenMaterialPropertyColor : TweenValueColor<Renderer>
    {
        public TweenMaterialPropertyColor SetMaterialMode(TweenMaterialMode materialMode)
        {
            MaterialData.Mode = materialMode;
            return this;
        }

        public TweenMaterialPropertyColor SetMaterialIndex(int materialIndex)
        {
            MaterialData.Index = materialIndex;
            return this;
        }

        public TweenMaterialPropertyColor SetPropertyName(string propertyName)
        {
            MaterialData.Property = propertyName;
            return this;
        }
    }

    public static partial class UTween
    {
        public static TweenMaterialPropertyColor Color(Renderer renderer, string propertyName, Color to, float duration)
        {
            var tweener = Color(renderer, 0, propertyName, to, duration);
            return tweener;
        }

        public static TweenMaterialPropertyColor Color(Renderer renderer, int materialIndex, string propertyName, Color to, float duration)
        {
            var tweener = Create<TweenMaterialPropertyColor>()
                .SetTarget(renderer)
                .SetMaterialIndex(materialIndex)
                .SetPropertyName(propertyName)
                .SetCurrent2From()
                .SetTo(to)
                .SetDuration(duration)
                .Play() as TweenMaterialPropertyColor;
            return tweener;
        }

        public static TweenMaterialPropertyColor Color(Renderer renderer, string propertyName, Color from, Color to, float duration)
        {
            var tweener = Color(renderer, 0, propertyName, from, to, duration);
            return tweener;
        }

        public static TweenMaterialPropertyColor Color(Renderer renderer, int materialIndex, string propertyName, Color from, Color to, float duration)
        {
            var tweener = Create<TweenMaterialPropertyColor>()
                .SetTarget(renderer)
                .SetMaterialIndex(materialIndex)
                .SetPropertyName(propertyName)
                .SetFrom(from)
                .SetTo(to)
                .SetDuration(duration)
                .Play() as TweenMaterialPropertyColor;
            return tweener;
        }

        public static TweenMaterialPropertyColor Color(Renderer renderer, string propertyName, Gradient gradient, float duration)
        {
            var tweener = Color(renderer, 0, propertyName, gradient, duration);
            return tweener;
        }

        public static TweenMaterialPropertyColor Color(Renderer renderer, int materialIndex, string propertyName, Gradient gradient, float duration)
        {
            var tweener = Create<TweenMaterialPropertyColor>()
                .SetTarget(renderer)
                .SetMaterialIndex(materialIndex)
                .SetPropertyName(propertyName)
                .SetColorMode(ColorMode.Gradient)
                .SetGradient(gradient)
                .SetDuration(duration)
                .Play() as TweenMaterialPropertyColor;
            return tweener;
        }
    }

    public static partial class RendererExtension
    {
        public static TweenMaterialPropertyColor TweenColor(this Renderer renderer, string propertyName, Color to, float duration)
        {
            var tweener = UTween.Color(renderer, propertyName, to, duration);
            return tweener;
        }

        public static TweenMaterialPropertyColor TweenColor(this Renderer renderer, int materialIndex, string propertyName, Color to, float duration)
        {
            var tweener = UTween.Color(renderer, materialIndex, propertyName, to, duration);
            return tweener;
        }

        public static TweenMaterialPropertyColor TweenColor(this Renderer renderer, string propertyName, Color from, Color to, float duration)
        {
            var tweener = UTween.Color(renderer, propertyName, from, to, duration);
            return tweener;
        }

        public static TweenMaterialPropertyColor TweenColor(this Renderer renderer, int materialIndex, string propertyName, Color from, Color to, float duration)
        {
            var tweener = UTween.Color(renderer, materialIndex, propertyName, from, to, duration);
            return tweener;
        }

        public static TweenMaterialPropertyColor TweenColor(this Renderer renderer, string propertyName, Gradient gradient, float duration)
        {
            var tweener = UTween.Color(renderer, propertyName, gradient, duration);
            return tweener;
        }

        public static TweenMaterialPropertyColor TweenColor(this Renderer renderer, int materialIndex, string propertyName, Gradient gradient, float duration)
        {
            var tweener = UTween.Color(renderer, materialIndex, propertyName, gradient, duration);
            return tweener;
        }
    }

    #endregion
}
