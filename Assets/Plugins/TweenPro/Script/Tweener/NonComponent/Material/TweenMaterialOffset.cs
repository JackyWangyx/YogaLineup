using UnityEngine;

namespace Aya.TweenPro
{
    public partial class TweenMaterialOffset : TweenValueVector2<Material>
    {
        public string Property;

        public override Vector2 Value
        {
            get => Target.GetTextureOffset(Property);
            set => Target.SetTextureOffset(Property, value);
        }
    }

    #region Extension

    public partial class TweenMaterialOffset : TweenValueVector2<Material>
    {
        public TweenMaterialOffset SetPropertyName(string propertyName)
        {
            Property = propertyName;
            return this;
        }
    }

    public static partial class UTween
    {
        public static TweenMaterialOffset Offset(Material material, string propertyName, Vector2 to, float duration)
        {
            var tweener = Create<TweenMaterialOffset>()
                .SetTarget(material)
                .SetPropertyName(propertyName)
                .SetCurrent2From()
                .SetTo(to)
                .SetDuration(duration)
                .Play() as TweenMaterialOffset;
            return tweener;
        }

        public static TweenMaterialOffset Offset(Material material, string propertyName, Vector2 from, Vector2 to, float duration)
        {
            var tweener = Create<TweenMaterialOffset>()
                .SetTarget(material)
                .SetPropertyName(propertyName)
                .SetFrom(from)
                .SetTo(to)
                .SetDuration(duration)
                .Play() as TweenMaterialOffset;
            return tweener;
        }
    }

    public static partial class MaterialExtension
    {
        public static TweenMaterialOffset TweenOffset(this Material material, string propertyName, Vector2 to, float duration)
        {
            var tweener = UTween.Offset(material, propertyName, to, duration);
            return tweener;
        }

        public static TweenMaterialOffset TweenOffset(this Material material, string propertyName, Vector2 from, Vector2 to, float duration)
        {
            var tweener = UTween.Offset(material, propertyName, from, to, duration);
            return tweener;
        }
    }

    #endregion
}
