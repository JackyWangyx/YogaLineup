#if UTWEEN_TEXTMESHPRO
using UnityEngine;

namespace Aya.TweenPro
{
    public interface ITMPCharacterModifier
    {
        bool ChangeGeometry { get; }
        bool ChangeColor { get; }

        void ModifyGeometry(int characterIndex, ref Vector3[] vertices, float progress);
        void ModifyColor(int characterIndex, ref Color32[] colors, float progress);
    }
}
#endif