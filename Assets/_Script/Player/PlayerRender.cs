using Aya.Extension;
using UnityEngine;

public class PlayerRender : PlayerBase
{
    public Transform RenderTrans;

    [SubPoolInstance] public GameObject RenderInstance { get; set; }

    public override void InitComponent()
    {
        YogaGirlList.Clear();
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

            var playerRendererPrefab = AvatarSetting.Ins.SelectedAvatarList[rank];
            RefreshRender(playerRendererPrefab);

            this.ExecuteNextFrame(() =>
            {
                if (data.ChangeFx != null && State.PointChanged)
                {
                    SpawnFx(data.ChangeFx, RenderTrans);
                }
            });
        }
    }

    public void AddRender(GameObject prefab, float Size)
    {
        var trans = RenderTrans;
        trans.SetLocalPositionZ((YogaGirlList.Count - 1) * Size);
        var girl = GamePool.Spawn(prefab, RenderTrans);
        var animator = girl.GetComponentInChildren<Animator>();
        YogaGirlList.Add(animator);

        Play(CurrentClip, animator);
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
        foreach(var girl in YogaGirlList)
        {
            GamePool.DeSpawn(girl);
        }
        RenderInstance = null;
    }
}
