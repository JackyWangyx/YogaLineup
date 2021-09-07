using System;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Aya.TweenPro
{
    [Tweener("Material Alpha", "Material", "Material Icon")]
    [Serializable]
    public partial class TweenMaterialPropertyBlockAlpha : TweenValueFloat<Renderer>
    {
        public TweenMaterialData MaterialData;

        public override float Value
        {
            get
            {
                MaterialData.Cache(Target);
                return MaterialData.GetColor().a;
            }
            set
            {
                MaterialData.Cache(Target);
                var color = MaterialData.GetColor();
                color.a = value;
                MaterialData.SetColor(color);
            }
        }

        public override void Reset()
        {
            base.Reset();
            MaterialData.Reset();
        }
    }

#if UNITY_EDITOR

    public partial class TweenMaterialPropertyBlockAlpha : TweenValueFloat<Renderer>
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

    public partial class TweenMaterialPropertyBlockAlpha : TweenValueFloat<Renderer>
    {
        public TweenMaterialPropertyBlockAlpha SetMaterialMode(TweenMaterialMode materialMode)
        {
            MaterialData.Mode = materialMode;
            return this;
        }

        public TweenMaterialPropertyBlockAlpha SetMaterialIndex(int materialIndex)
        {
            MaterialData.Index = materialIndex;
            return this;
        }

        public TweenMaterialPropertyBlockAlpha SetPropertyName(string propertyName)
        {
            MaterialData.Property = propertyName;
            return this;
        }
    }

    public static partial class UTween
    {
        public static TweenMaterialPropertyBlockAlpha Alpha(Renderer renderer, string propertyName, float to, float duration)
        {
            var tweener = Alpha(renderer, 0, propertyName, to, duration);
            return tweener;
        }

        public static TweenMaterialPropertyBlockAlpha Alpha(Renderer renderer, int materialIndex, string propertyName, float to, float duration)
        {
            var tweener = Create<TweenMaterialPropertyBlockAlpha>()
                .SetTarget(renderer)
                .SetMaterialIndex(materialIndex)
                .SetPropertyName(propertyName)
                .SetCurrent2From()
                .SetTo(to)
                .SetDuration(duration)
                .Play() as TweenMaterialPropertyBlockAlpha;
            return tweener;
        }

        public static TweenMaterialPropertyBlockAlpha Alpha(Renderer renderer, string propertyName, float from, float to, float duration)
        {
            var tweener = Alpha(renderer, 0, propertyName, from, to, duration);
            return tweener;
        }

        public static TweenMaterialPropertyBlockAlpha Alpha(Renderer renderer, int materialIndex, string propertyName, float from, float to, float duration)
        {
            var tweener = Create<TweenMaterialPropertyBlockAlpha>()
                .SetTarget(renderer)
                .SetMaterialIndex(materialIndex)
                .SetPropertyName(propertyName)
                .SetFrom(from)
                .SetTo(to)
                .SetDuration(duration)
                .Play() as TweenMaterialPropertyBlockAlpha;
            return tweener;
        }
    }

    public static partial class RendererExtension
    {
        public static TweenMaterialPropertyBlockAlpha TweenAlpha(this Renderer renderer, string propertyName, float to, float duration)
        {
            var tweener = UTween.Alpha(renderer, propertyName, to, duration);
            return tweener;
        }

        public static TweenMaterialPropertyBlockAlpha TweenAlpha(this Renderer renderer, int materialIndex, string propertyName, float to, float duration)
        {
            var tweener = UTween.Alpha(renderer, materialIndex, propertyName, to, duration);
            return tweener;
        }

        public static TweenMaterialPropertyBlockAlpha TweenAlpha(this Renderer renderer, string propertyName, float from, float to, float duration)
        {
            var tweener = UTween.Alpha(renderer, propertyName, from, to, duration);
            return tweener;
        }

        public static TweenMaterialPropertyBlockAlpha TweenAlpha(this Renderer renderer, int materialIndex, string propertyName, float from, float to, float duration)
        {
            var tweener = UTween.Alpha(renderer, materialIndex, propertyName, from, to, duration);
            return tweener;
        }
    }

    #endregion
}
