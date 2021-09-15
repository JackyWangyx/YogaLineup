﻿using System.Collections.Generic;
using Aya.Util;
using UnityEngine;

public class LevelManager : GameEntity<LevelManager>
{
    public int TestLevelIndex = 1;
    public int StartRandIndex = -1;
    public List<Level> RandList;

    public new Level Level { get; set; }

    public virtual void NextLevel()
    {
        Save.LevelIndex++;
        if (Save.LevelIndex >= StartRandIndex && StartRandIndex > 0)
        {
            Save.RandLevelIndex.Value = RandUtil.RandInt(0, RandList.Count);
        }

        LevelStart();
    }

    public virtual void LevelStart()
    {
        UI.HideAll();
        GamePool.DeSpawnAll();
        EffectPool.DeSpawnAll();
        var index = Save.LevelIndex.Value;

        if (Level != null)
        {
            Level = null;
        }

        if (TestLevelIndex > 0)
        {
            index = TestLevelIndex;
        }
        else if (index >= StartRandIndex && StartRandIndex > 0)
        {
            index = StartRandIndex;
        }

        var levelPrefab = Resources.Load<Level>("Level/Level_" + index.ToString("D2"));
        Level = GamePool.Spawn(levelPrefab);
        Level.Trans.SetParent(null);
        Level.Init();

        Game.Enter<GameReady>();
    }
}
