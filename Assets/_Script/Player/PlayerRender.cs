using Aya.Extension;
using UnityEngine;

public class PlayerRender : PlayerBase
{
    public Transform RenderTrans;

    public GameObject RenderInstance { get; set; }

    public override void InitComponent()
    {
        RefreshRender(State.Point);
    }

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

    public void RefreshRender(GameObject prefab)
    {
        if (RenderInstance != null)
        {
            GamePool.DeSpawn(RenderInstance);
            RenderInstance = null;
        }

        RenderInstance = GamePool.Spawn(prefab, RenderTrans);

        this.ExecuteNextFrame(() =>
        {
            CacheComponent();
            Play(CurrentClip);
        });
    }
}
