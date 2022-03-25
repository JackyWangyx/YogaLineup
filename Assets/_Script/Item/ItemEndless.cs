﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Aya.Extension;
using Aya.TweenPro;

public class ItemEndless : ItemBase<Player>
{
    public GirlFollow Girl;
    public List<Transform> RandomSpawnList;
    public int Index { get; set; }
    public List<GirlFollow> GirlList { get; set; }
    public List<string> StrList { get; set; }

    protected override void Awake()
    {
        GirlList = new List<GirlFollow>();
        StrList = new List<string>();
    }

    public override void Init()
    {
        base.Init();
        foreach (var girl in GirlList)
        {
            GamePool.DeSpawn(girl.gameObject);
        }
        GirlList.Clear();
        StrList.Clear();
    }

    public override void OnTargetEffect(Player target)
    {
        for(var i = 0; i < GirlList.Count; i++)
        {
            if (Game.YogaGirlList.Count <= 0)
            {
                Game.Enter<GameWin>();
                return;
            }
            var pos = GirlList[i].transform.position;
            var str = StrList[i];
            var girl = Game.YogaGirlList.Last();
            girl.transform.SetParent(transform);
            girl.Animator.Play(str, 0, 1f);
            UTween.Position(girl.transform, girl.transform.position, pos, 1f)
                    .SetOnStop(() =>
                    {
                        girl.Animator.transform.localPosition = Vector3.zero;
                    });
            Game.YogaGirlList.Remove(girl);
        }
    }

    public void InitGirl()
    {
        Index = Level.Level.RainbowIndex;
        var count = 1;
        if (Index >= 20)
            count = 5;
        else if (Index >= 15)
            count = 4;
        else if (Index >= 10)
            count = 3;
        else if (Index >= 5)
            count = 2;
        UpdateAnimation(count);
    }

    public void UpdateAnimation(int count)
    {
        StrList = Player.Control._yogaList.Random(count);
        var transList = RandomSpawnList.Random(count);
        for (var i = 0; i < count; i++)
        {
            var str = StrList[i];
            var trans = transList[i];
            var girl = GamePool.Spawn(Girl, trans);
            girl.Animator.speed = 0;
            girl.Animator.Play(str, 0, 1f);
            GirlList.Add(girl);
        }
        Level.Level.RainbowIndex++;
    }
}