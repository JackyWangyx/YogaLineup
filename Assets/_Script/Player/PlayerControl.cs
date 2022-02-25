using Aya.Extension;
using UnityEngine;

public abstract class PlayerControl : PlayerBase
{
    public virtual void Update()
    {
        var deltaTime = DeltaTime;
        if (Game.GamePhase != GamePhaseType.Gaming) return;
        if (!State.EnableInput) return;
        UpdateImpl(deltaTime);
    }

    public virtual void UpdateImpl(float deltaTime)
    {

    }
}
