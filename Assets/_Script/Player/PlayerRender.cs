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

            if (RenderInstance != null)
            {
                GamePool.DeSpawn(RenderInstance);
                RenderInstance = null;
            }

            var playerRendererPrefab = AvatarSetting.Ins.SelectedAvatarList[rank];
            RenderInstance = GamePool.Spawn(playerRendererPrefab, RenderTrans);

            this.ExecuteNextFrame(() =>
            {
                CacheComponent();
                Play(CurrentClip);

                if (data.ChangeFx != null && State.PointChanged)
                {
                    SpawnFx(data.ChangeFx, RenderTrans);
                }
            });
        }
    }
}
