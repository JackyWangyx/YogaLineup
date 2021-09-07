using Aya.Data.Persistent;

public class SaveManager : GameEntity<SaveManager>
{
    public sInt LevelIndex = new sInt("LevelIndex");
    public sInt Coin = new sInt("Coin");
}
