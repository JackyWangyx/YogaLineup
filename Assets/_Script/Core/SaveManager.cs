using System;
using Aya.Data.Persistent;

[Serializable]
public class SaveSlotData
{
    // Save slot data
}

public class SaveManager : GameEntity<SaveManager>
{
    public SaveSlotData Data;

    public sInt LevelIndex = new sInt("LevelIndex", 1);
    public sInt RandLevelIndex = new sInt("RandLevelIndex", 0);

    public sInt Coin;
    public sInt Key;

    protected override void Awake()
    {
        base.Awake();

        Coin = new sInt("Coin", GetSetting<GeneralSetting>().DefaultCoin);
        Key = new sInt("Key", GetSetting<GeneralSetting>().DefaultKey);

        Load();
    }

    public void Load()
    {
        SaveData.Load("Save");
        Data = SaveData.Get<SaveSlotData>("Save", "Save");
    }

    public void SaveSync()
    {
        SaveData.Set("Save", "Save", Data);
        SaveData.Save("Save");
    }

    public void SaveAsync(Action onDone = null)
    {
        SaveData.Set("Save", "Save", Data);
        SaveData.SaveAsync("Save", onDone);
    }
}
