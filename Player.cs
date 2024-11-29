public class Player
{
    private readonly ThrowHandler throwHandler = new ThrowHandler();
    public string Name { get; private set; }
    public IThrow PowerType { get; private set; }
    public IThrow StrategyType { get; private set; }

    public Player(string playerName, IThrow powerType, IThrow strategyType)  
    {
        Name = playerName;
        PowerType = powerType;
        StrategyType = strategyType;
    }

    public void UpdateThrowSettings(IThrow powerType, IThrow strategyType)
    {
        PowerType = powerType;
        StrategyType = strategyType;
    }

    public int PerformThrow(BowlingLane lane)
    {
        return throwHandler.PerformThrow(Name, PowerType, lane);
    }
}