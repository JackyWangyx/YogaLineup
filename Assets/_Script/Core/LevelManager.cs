using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class LevelManager : GameEntity<LevelManager>
{
    [FoldoutGroup("Level")] public int LevelIndex = 1;
    [FoldoutGroup("Level")] public int LevelCount;

    [FoldoutGroup("Random")] public int StartRandIndex = -1;
    [FoldoutGroup("Random")] public List<Level> RandList;

    public new Level Level { get; set; }

    public virtual void NextLevel()
    {
        LevelIndex++;
        if (LevelIndex > LevelCount) LevelIndex = 1;

        LevelStart();
    }

    public virtual void LevelStart()
    {
        UI.HideAll();
        GamePool.DeSpawnAll();
        EffectPool.DeSpawnAll();
        var index = LevelIndex;

        if (Level != null)
        {
            Level = null;
        }

        Level = GamePool.Spawn(Resources.Load<Level>("Level/Level_" + index.ToString("D2")));
        Level.Trans.SetParent(null);
        Level.Init();

        Game.Enter<GameReady>();
    }
}
