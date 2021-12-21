using UnityEngine;

namespace Aya.TweenPro
{
    public interface ITextCharacterModifier
    {
        void Modify(int characterIndex, ref UIVertex[] vertices);
    }
}