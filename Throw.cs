using System;
using System.Drawing;
using System.Numerics;

public interface IThrow
{
    IStrategy Strategy { get; }
    string Name { get; }
    string Description { get; }
    int Number { get; }
}

public interface IStrategy
{
    string Name { get; }
    int Number { get; }
    (bool hit, string result) Spin();
}

public class WeakPower : IThrow
{
    public IStrategy Strategy { get; }
    public string Name => "Weak";
    public string Description => "Less power but more control";
    public int Number => 10;

    public WeakPower(IStrategy strategy)
    {
        Strategy = strategy;
    }
}

public class StrongPower : IThrow
{
    public IStrategy Strategy { get; }
    public string Name => "Strong";
    public string Description => "More power but less control";
    public int Number => 100;

    public StrongPower(IStrategy strategy)
    {
        Strategy = strategy;
    }
}

public class LeftHandSpin : IStrategy
{
    public string Name => "Left Hand Spin";
    public int Number => 0;

    public (bool hit, string result) Spin()
    {
        Random random = new Random();
        int spinChance = random.Next(1, 101);
        bool willHit = spinChance <= 70;
        return (willHit, willHit ? "Left Hand Spin" : "\u001b[91mOps! You slipped!\u001b[0m");
    }
}

public class SuperSpin : IStrategy
{
    public string Name => "Super spin";
    public int Number => 50;

    public (bool hit, string result) Spin()
    {
        Random random = new Random();
        int spinChance = random.Next(1, 101);
        bool willHit = spinChance <= 60;
        return (willHit, willHit ? "Super spin" : "\u001b[91mOh no... The ball went out of the lane..\u001b[0m");
    }
}

public class RightHandSpin : IStrategy
{
    public string Name => "Right spin";
    public int Number => 100;

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
        if (!hit)
        {
            Console.WriteLine(result);
            return 0;
        }

        int pinsDown = lane.MakeThrow(power.Number, power.Strategy.Number);
        Console.WriteLine($"Pins down this throw: {pinsDown}");
        return pinsDown;
    }
}
