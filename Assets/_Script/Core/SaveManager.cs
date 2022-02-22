using System;
using Aya.Data.Persistent;

[Serializable]
public class SaveSlotData : sObject
{
    // Save slot data

    public SaveSlotData(string key) : base(key)
    {
    }
}

public class SaveManager : GameEntity<SaveManager>
{
    public sInt LevelIndex = new sInt(nameof(LevelIndex), 1);
    public sInt RandLevelIndex = new sInt(nameof(RandLevelIndex), 0);
    public SaveSlotData Data { get; set; }

    public sInt Coin;
    public sInt Key;


    protected override void Awake()
    {
        base.Awake();
        Coin = new sInt(nameof(Coin), GetSetting<GeneralSetting>().DefaultCoin);
        Key = new sInt(nameof(Key), GetSetting<GeneralSetting>().DefaultKey);

        Load();
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        SaveSync();
    }

    public void Load()
    {
        Data = sObject<SaveSlotData>.Load(nameof(SaveSlotData));
        if (Data == null) Data = new SaveSlotData(nameof(SaveSlotData));
    }

    public void SaveSync()
    {
        sObject<SaveSlotData>.Save(Data);
    }
}
