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
        
        // Analysera kägelpositioner
        int leftPins = pins.Count(p => p.X < 2);
        int rightPins = pins.Count(p => p.X > 1);
        int backPins = pins.Count(p => p.Y == 3);
        
        // Välj strategi baserat på kägelpositioner
        IStrategy strategy = (leftPins, rightPins, backPins) switch
        {
            var (l, r, b) when b >= 2 => new SuperSpin(),      // Många käglor bak -> SuperSpin
            var (l, r, _) when l > r => new LeftHandSpin(),    // Fler käglor vänster -> LeftHandSpin
            var (l, r, _) when r > l => new RightHandSpin(),   // Fler käglor höger -> RightHandSpin
            _ => new SuperSpin()                               // Default om jämnt fördelat
        };

        // Välj power baserat på antal kvarvarande käglor
        PowerType = pins.Count() <= 5 
            ? new WeakPower(strategy)    // Få käglor -> Weak för bättre kontroll
            : new StrongPower(strategy); // Många käglor -> Strong för maximal effekt

        return throwHandler.PerformThrow(Name, PowerType, lane);
    }
}
