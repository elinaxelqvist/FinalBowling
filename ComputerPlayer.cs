using System.Runtime.CompilerServices;
public class ComputerPlayer
{
    private readonly ThrowHandler throwHandler = new();
    private readonly Random random = new();
    public string Name { get; }
    public IThrow PowerType { get; private set; }

    public ComputerPlayer(string name, IThrow powerType)
    {
        Name = name;
        PowerType = powerType;
    }

    public void UpdateThrowSettings(IThrow powerType)
    {
        PowerType = powerType;
    }

    public int PerformThrow(BowlingLane lane)
    {
        var pins = lane.GetPins();
        IStrategy strategy;
     
        if (pins.Count() < 10)
        {
            do
            {
                strategy = random.Next(1, 4) switch
                {
                    1 => new LeftHandSpin(),
                    2 => new SuperSpin(),
                    _ => new RightHandSpin()
                };
            } while (strategy.GetType() == PowerType?.Strategy?.GetType());
        }
        else
        {
            strategy = random.Next(1, 4) switch
            {
                1 => new LeftHandSpin(),
                2 => new SuperSpin(),
                _ => new RightHandSpin()
            };
        }
        
        PowerType = random.Next(1, 3) == 1
            ? new WeakPower(strategy)
            : new StrongPower(strategy);

        return throwHandler.PerformThrow(Name, PowerType, lane);
    }
}
