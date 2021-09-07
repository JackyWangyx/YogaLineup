using UnityEngine;

namespace Aya.TweenPro
{
    public interface ICharacterModifier
    {
        void Modify(int characterIndex, ref UIVertex[] vertices, float progress);
    }
}