using Aya.Data.Persistent;

public class SaveManager : GameEntity<SaveManager>
{
    public sInt LevelIndex = new sInt("LevelIndex", 1);
    public sInt RandLevelIndex = new sInt("RandLevelIndex", 0);

    public sInt Coin;
    public sInt Key;

    protected override void Awake()
    {
        base.Awake();
        Coin = new sInt("Coin", GetSetting<GeneralSetting>().DefaultCoin);
        Key = new sInt("Key", GetSetting<GeneralSetting>().DefaultKey);
    }
}
