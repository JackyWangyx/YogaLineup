using Aya.Data.Persistent;

public class SaveManager : GameEntity<SaveManager>
{
    public sInt LevelIndex = new sInt("LevelIndex", 1);
    public sInt RandLevelIndex = new sInt("RandLevelIndex", 0);

    public sInt Coin = new sInt("Coin");
}
