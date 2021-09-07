using System;
using System.Collections;
using System.Collections.Generic;
using Aya.Extension;
using Aya.Particle;
using Aya.Singleton;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum GamePhase
{
    Ready,
    Gaming,
    Win,
    Fail,

    Waiting,
}

[Serializable]
public class PlayerData
{
    public int Point;
    public GameObject Player;
    public GameObject ChangeFx;
    public Color Color;
}

public class GameManager : GameEntity<GameManager>
{
    public int LevelIndex = 1;
    public int LevelCount;

    public List<PlayerData> PlayerDatas;

    public new Level Level { get; set; }

    public new Player Player;
    public GamePhase Phase { get; set; }

    public virtual void Start()
    {
        LevelStart();
    }

    public virtual void NextLevel()
    {
        LevelIndex++;
        if (LevelIndex > LevelCount) LevelIndex = 1;

        LevelStart();
    }

    public virtual void LevelStart()
    {
        UIController.Ins.HideAll();
        ParticleSpawner.EntityPool.DeSpawnAll();
        var index = LevelIndex;

        if (Level != null)
        {
            DestroyImmediate(Level.gameObject);
            Level = null;
        }

        Level = Instantiate(Resources.Load<Level>("Level/Level_" + index.ToString("D2")), Vector3.zero, Quaternion.identity);
        Level.Init();

        GameReady();
    }

    public virtual void GameReady()
    {
        Phase = GamePhase.Ready;
        CameraManager.Ins.SwitchCamera("Ready");
        GameStart();
    }

    public virtual void GameStart()
    {
        Phase = GamePhase.Gaming;
        CameraManager.Ins.SwitchCamera("Game");
        UIController.Ins.Show<UIGame>();
        Player.StartRun();
    }

    public void Update()
    {
        GameUpdate();
    }

    public virtual void GameUpdate()
    {
        if (Phase == GamePhase.Gaming)
        {
            if (Player.Point == 0 && Player.PointChanged)
            {
                GameLose();
            }
        }
    }

    public virtual void GameWin()
    {
        Phase = GamePhase.Win;
        // CameraManager.Ins.SwitchCamera("Finish");
        UIController.Ins.Show<UIWin>();
        Player.Win();
    }

    public virtual void GameLose()
    {
        Phase = GamePhase.Fail;
        // CameraManager.Ins.SwitchCamera("Finish");
        UIController.Ins.Show<UILose>();
    }
}
