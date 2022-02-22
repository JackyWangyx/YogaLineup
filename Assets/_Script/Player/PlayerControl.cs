using Aya.Extension;
using UnityEngine;

public class PlayerControl : PlayerBase
{
    public void Update()
    {
        var deltaTime = DeltaTime;
        if (Game.GamePhase != GamePhaseType.Gaming) return;
        UpdateImpl(deltaTime);
    }

    public virtual void UpdateImpl(float deltaTime)
    {

    }
}
