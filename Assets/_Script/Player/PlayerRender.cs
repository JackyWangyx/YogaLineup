using Aya.Extension;
using UnityEngine;

public class PlayerRender : PlayerBase
{
    public Transform RenderTrans;
    public Transform GirlListTrans;

    [SubPoolInstance] public GameObject RenderInstance { get; set; }

    public override void InitComponent()
    {
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
                var TransZ = Game.YogaGirlList.Count * Size + Size;
                TransZ *= direction;
                var girl = GamePool.Spawn(prefab, GirlListTrans);
                var target = Player.Render.RenderTrans;
                if (Game.YogaGirlList.Count > 0)
                    target = Game.YogaGirlList.Last().RendererTrans;
                girl.AddComponent<GirlFollow>().Init(TransZ, 0.1f * Game.YogaGirlList.Count, target);
                var GirlFollow = girl.GetComponentInChildren<GirlFollow>();
                Game.YogaGirlList.Add(GirlFollow);
            }
        }
        else
        {
            for (var i = 0; i > value; i--)
            {
                var girl = Game.YogaGirlList[Game.YogaGirlList.Count - 1];
                Game.YogaGirlList.Remove(girl);
                GamePool.DeSpawn(girl.gameObject);
            }
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
            GamePool.DeSpawn(girl);
        }
        RenderInstance = null;
    }
}
