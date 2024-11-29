using System;
using System.Drawing;
using System.Numerics;


// KRAV #:
// 1: Bridge pattern
// 2: IThrow definerar det gränssnitt som power implementerar. Power har en strategi. Strategierna har ett annat gränssnitt, Istratetgy.
//  Detta skapar två lager av abstraktion och tillsamans skapar power och stategi ett kast
// 3: Vi använder oss av bridge pattern för att minska risken för duplicerad kod samt för att säkerställa att att power har en strategi.
//Om man i framtiden vill lägga till kast med andra strategier så kan vi återanvända kod och göra implementationer, utan att det påverkar befintlig kod.


// KRAV #:
// 1: Stratetgy pattern
// 2: Används genom gränssnittet Istratetgy som olika strategier implementerar för att skapa olika beteenden, strategierna används sedan vid skapandets av ett kast
// 3: Stratetgy pattern används för att spelaren ska kunna välja olika strategier i runtime, för varje kast
public interface IThrow
{
    IStrategy Strategy { get; }
    public string Name { get; }
    public string Description { get; }
    public int Number { get; }
}

public interface IStrategy
{
    public string Name { get; }
    public int Number { get; }
    (bool hit, string result) Spin();
}


class WeakPower : IThrow  
{
    public string Name { get; private set; }
    public string Description { get; private set; }
    public int Number { get; private set; }
    public IStrategy Strategy { get; private set; }

    public WeakPower(IStrategy strategy)
    {
        Name = "Weak";
        Description = "For Pins in the front row";
        Number = 10;
        Strategy = strategy;
    }
}

class StrongPower : IThrow
{
    public string Name { get; private set; }
    public string Description { get; private set; }
    public int Number { get; private set; }
    public IStrategy Strategy { get; private set; }

    public StrongPower(IStrategy strategy)
    {
        Name = "Strong";
        Description = "For Pins in the back row";
        Number = 100;
        Strategy = strategy;
    }
}

class LeftHandSpin : IStrategy   
{
    public string Name { get; private set; }
    public int Number { get; private set; }

    public LeftHandSpin()
    {
        Name = "Left Hand Spin";
        Number = 0;
    }

    public (bool hit, string result) Spin()
    {
        Random random = new Random();
        int spinChance = random.Next(1, 101);
        bool willHit = spinChance <= 70;
        return (willHit, willHit ? "Left Hand Spin" : "\u001b[91mOps! You slipped!\u001b[0m");
    }
}

class SuperSpin : IStrategy
{
    public string Name { get; private set; }
    public int Number { get; private set; }

    public SuperSpin()
    {
        Name = "Super spin";
        Number = 50;
    }

    public (bool hit, string result) Spin()
    {
        Random random = new Random();
        int spinChance = random.Next(1, 101);
        bool willHit = spinChance <= 60;
        return (willHit, willHit ? "Super spin" : "\u001b[91mOh no... The ball went out of the lane..\u001b[0m");
    }
}

class RightHandSpin : IStrategy   
{
    public string Name { get; private set; }
    public int Number { get; private set; }

    public RightHandSpin()
    {
        Name = "Right spin";
        Number = 100;
    }

    public (bool hit, string result) Spin()
    {
        Random random = new Random();
        int spinChance = random.Next(1, 101);
        bool willHit = spinChance <= 60;
        return (willHit, willHit ? "Right Spin" : "\u001b[91mWops, too much power! The ball got in another persons lane..\u001b[0m");
    }
}

public class ThrowHandler
{
    public int PerformThrow(string name, IThrow power, BowlingLane lane)
    {
        Console.WriteLine($"{name} is throwing with {power.Name} Power and {power.Strategy.Name}!");

        var (hit, result) = power.Strategy.Spin();
        if (hit)
        {
            int pinsDown = lane.MakeThrow(power.Number, power.Strategy.Number);
            Console.WriteLine($"Pins down this throw: {pinsDown}");
            return pinsDown;
        }
        else
        {
            Console.WriteLine($"{result}");
            lane.Print();
            return 0;
        }
    }
}
