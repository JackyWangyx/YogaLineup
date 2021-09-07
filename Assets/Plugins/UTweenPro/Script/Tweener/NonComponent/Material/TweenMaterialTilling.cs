using UnityEngine;

namespace Aya.TweenPro
{
    public partial class TweenMaterialTilling : TweenValueVector2<Material>
    {
        public string Property;

        public override Vector2 Value
        {
            get => Target.GetTextureScale(Property);
            set => Target.SetTextureScale(Property, value);
        }
    }

    #region Extension

    public partial class TweenMaterialTilling : TweenValueVector2<Material>
    {
        public TweenMaterialTilling SetPropertyName(string propertyName)
        {
            Property = propertyName;
            return this;
        }
    }

    public static partial class UTween
    {
        public static TweenMaterialTilling Tilling(Material material, string propertyName, Vector2 to, float duration)
        {
            var tweener = Create<TweenMaterialTilling>()
                .SetTarget(material)
                .SetPropertyName(propertyName)
                .SetCurrent2From()
                .SetTo(to)
                .SetDuration(duration)
                .Play() as TweenMaterialTilling;
            return tweener;
        }

        public static TweenMaterialTilling Tilling(Material material, string propertyName, Vector2 from, Vector2 to, float duration)
        {
            var tweener = Create<TweenMaterialTilling>()
                .SetTarget(material)
                .SetPropertyName(propertyName)
                .SetFrom(from)
                .SetTo(to)
                .SetDuration(duration)
                .Play() as TweenMaterialTilling;
            return tweener;
        }
    }

    public static partial class MaterialExtension
    {
        public static TweenMaterialTilling TweenTilling(this Material material, string propertyName, Vector2 to, float duration)
        {
            var tweener = UTween.Tilling(material, propertyName, to, duration);
            return tweener;
        }

        public static TweenMaterialTilling TweenTilling(this Material material, string propertyName, Vector2 from, Vector2 to, float duration)
        {
            var tweener = UTween.Tilling(material, propertyName, from, to, duration);
            return tweener;
        }
    }

    #endregion
}
