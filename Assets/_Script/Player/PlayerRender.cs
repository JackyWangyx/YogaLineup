using Aya.Extension;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRender : PlayerBase
{
    public float GirlSpawnInterval = 0.5f;
    public Transform RenderTrans;
    public Transform GirlListTrans;
    private float lastWaitTime;

    [SubPoolInstance] public GameObject RenderInstance { get; set; }

    public override void InitComponent()
    {
        lastWaitTime = 0;
        Game.YogaGirlList.Clear();
        RenderTrans.SetLocalPositionX(0f);
        RefreshRender(State.Point);
    }

    //public void RefreshRender(int point)
    //{
    //    var datas = PlayerSetting.Ins.PlayerDatas;
    //    var rank = 0;
    //    var data = datas[0];
    //    for (var i = 0; i < datas.Count; i++)
    //    {
    //        if (point >= datas[i].Point)
    //        {
    //            data = datas[i];
    //            rank = i;
    //        }
    //    }

    //    if (State.Rank != rank)
    //    {
    //        State.Rank = rank;
    //        Self.Data = data;

    //        var playerRendererPrefab = AvatarSetting.Ins.SelectedAvatarList[rank];
    //        RefreshRender(playerRendererPrefab);

    //        this.ExecuteNextFrame(() =>
    //        {
    //            if (data.ChangeFx != null && State.PointChanged)
    //            {
    //                SpawnFx(data.ChangeFx, RenderTrans);
    //            }
    //        });
    //    }
    //}

    public void RefreshRender(int point)
    {
        var datas = PlayerSetting.Ins.PlayerDatas;
        var rank = 0;
        var data = datas[0];
        for (var i = 0; i < datas.Count; i++)
        {
            if (point >= datas[i].Point)
            {
                data = datas[i];
                rank = i;
            }
        }

        if (State.Rank != rank)
        {
            State.Rank = rank;
            Self.Data = data;

            State.YoGaGirlPrefab = AvatarSetting.Ins.SelectedAvatarList[rank];
            RefreshRender(State.YoGaGirlPrefab);

            this.ExecuteNextFrame(() =>
            {
                if (data.ChangeFx != null && State.PointChanged)
                {
                    SpawnFx(data.ChangeFx, RenderTrans);
                }
            });
        }
    }

    public void AddRender(GameObject prefab, float Size, int value)
    {
        var IsAdd = value > 0 ? true : false;
        var direction = Game.PlayerFirst ? -1 : 1;
        if (IsAdd)
        {
            for(var i = 0; i < value; i++)
            {
                var waitTime = i * GirlSpawnInterval;
                lastWaitTime += waitTime;
                if (lastWaitTime > 0 && waitTime == 0)
                    lastWaitTime += GirlSpawnInterval;
                this.ExecuteDelay(() =>
                {
                    lastWaitTime -= waitTime;
                    var TransZ = Game.YogaGirlList.Count * Size + Size;
                    TransZ *= direction;
                    var girl = GamePool.Spawn(prefab, GirlListTrans);
                    var target = Player.Render.RenderTrans;
                    if (Game.YogaGirlList.Count > 0)
                        target = Game.YogaGirlList.Last().transform;
                    var follow = girl.GetOrAddComponent<GirlFollow>();
                    follow.Init(TransZ, target);
                    Game.YogaGirlList.Add(follow);
                }, lastWaitTime);
            }
        }
        else
        {
            var nowPos = Vector3.zero;
            for (var i = 0; i > value; i--)
            {
                if (Game.YogaGirlList.Count <= 0)
                {
                    Player.Lose();
                    return;
                }
                var girl = Game.YogaGirlList[0];
                nowPos = girl.transform.position;
                Game.YogaGirlList.Remove(girl);

                var Ins = girl.gameObject;
                this.ExecuteDelay(() => { GamePool.DeSpawn(Ins); }, 1f);
                //girl.IsDead = true;
                girl.Play("Yoga11");
                Destroy(girl, 1f);
                Player.Move.PathFollower.Distance -= Size;
                Player.Move.PathFollower.BlockDistance -= Size;
            }
            var index = 0;
            foreach(var girl in Game.YogaGirlList)
            {
                var target = Player.Render.RenderTrans;
                if (index > 0)
                    target = Game.YogaGirlList[index - 1].transform;
                //girl.Target = target;
                index++;
            }
            Player.transform.position = nowPos;
        }
    }

    public void RefreshRender(GameObject prefab)
    {
        DeSpawnRenderer();
        RenderInstance = GamePool.Spawn(prefab, RenderTrans);

        ComponentDic.ForEach(c => c.Value.CacheRendererComponent());
        Play(CurrentClip);
    }

    public void DeSpawnRenderer()
    {
        foreach(var girl in Game.YogaGirlList)
        {
            var follow = RenderInstance.GetComponent<PathFollowerGirl>();
            if (follow != null)
                Destroy(follow);
            GamePool.DeSpawn(girl.transform);
        }
        Game.YogaGirlList.Clear();
        GamePool.DeSpawn(Render.RendererTrans);
        RenderInstance = null;
    }
}
