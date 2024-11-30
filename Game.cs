using System.Text.Json;
public class Game
{
    private Score playerScore = new Score();
    private Score computerScore = new Score();
    private BowlingStatistics<GameHistory> gameHistory = new();
    private BowlingStatistics<PlayerFrequency> playerFrequency = new();
    private BowlingLane lane;
    private Player player;
    private ComputerPlayer computerPlayer;

    public Game()
    {
        Console.WriteLine("Welcome to Bowling Game!");
        Console.Write("Enter your name: ");
        string playerName = Console.ReadLine() ?? "Player";

        lane = new BowlingLane();
        IStrategy defaultStrategy = new SuperSpin();
        IThrow defaultPower = new WeakPower(defaultStrategy);
        player = new Player(playerName, defaultPower, defaultPower);
        computerPlayer = new ComputerPlayer("Computer", defaultPower, defaultPower);
    }

    public void PlayGame()
    {
        for (int round = 1; round <= 5; round++)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"\nROUND {round}");
            Console.ResetColor();

            lane = new BowlingLane();
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine($"\n=== {player.Name}'s Turn ===");
            Console.ResetColor();
            lane.Print();

            for (int i = 0; i < 2; i++)
            {
      
                Console.WriteLine("\nChoose your strategy:");
                Console.WriteLine("1. Left Hand Spin");
                Console.WriteLine("2. Super Spin, aiming for strike");
                Console.WriteLine("3. Right Hand Spin");
                Console.Write("Input (1-3): ");

                int strategyChoice;
                while (!int.TryParse(Console.ReadLine(), out strategyChoice) || strategyChoice < 1 || strategyChoice > 3)
                {
                    Console.WriteLine("Invalid input. Please enter 1, 2 or 3:");
                }

       
                Console.WriteLine("\nChoose your power:");
                Console.WriteLine("1. Weak");
                Console.WriteLine("2. Strong");
                Console.Write("Input (1-2): ");

                int powerChoice;
                while (!int.TryParse(Console.ReadLine(), out powerChoice) || powerChoice < 1 || powerChoice > 2)
                {
                    Console.WriteLine("Invalid input. Please enter 1 or 2:");
                }

                IStrategy strategy = strategyChoice switch
                {
                    1 => new LeftHandSpin(),
                    2 => new SuperSpin(),
                    _ => new RightHandSpin()
                };

                IThrow power = powerChoice == 1
                    ? new WeakPower(strategy)
                    : new StrongPower(strategy);

                player.UpdateThrowSettings(power, power);
                int playerThrowScore = player.PerformThrow(lane);
                playerScore.AddPoints(playerThrowScore);

                if (lane.AllPinsDown())
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"All pins are down!");
                    Console.ResetColor();
                    break;
                }
            }

            lane = new BowlingLane();
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine($"\n=== {computerPlayer.Name}'s Turn ===");
            Console.ResetColor();
            Console.WriteLine();

            for (int i = 0; i < 2; i++)
            {
                Thread.Sleep(1000);
                int computerThrowScore = computerPlayer.PerformThrow(lane);
                computerScore.AddPoints(computerThrowScore);

                if (lane.AllPinsDown())
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"All pins are down!");
                    Console.ResetColor();
                    break;
                }
            }

            ShowFrameStatistics();
        }

        Console.WriteLine("\nGame over! 5 rounds completed.");
        var (winner, playerTotal, computerTotal) = AnalyzeScores(playerScore, computerScore);

        Console.WriteLine("\nFinal Scores:");
        Console.WriteLine($"{player.Name}: {playerTotal} points");
        Console.WriteLine($"Computer: {computerTotal} points");

        if (winner == "Tie")
        {
            Console.WriteLine("\nIt's a tie!");
        }
        else
        {
            Console.WriteLine($"\n{winner} wins with {(winner == player.Name ? playerTotal : computerTotal)} points!");
            Console.WriteLine($"Winning margin: {Math.Abs(playerTotal - computerTotal)} points");
        }

    
        gameHistory.AddData(new GameHistory
        {
            PlayerName = player.Name,
            Score = playerTotal,
        });

    
        playerFrequency.AddData(new PlayerFrequency
        {
            PlayerName = player.Name,
        });


        Console.WriteLine("\nPress Enter to see more statistics...");
        Console.ReadLine();
        Console.Clear();


        gameHistory.ShowStatistics();

        Console.WriteLine("\nEnter player name to see their total games: ");
        string searchName = Console.ReadLine() ?? "";
        playerFrequency.ShowStatistics(searchName);
    }

    private void ShowFrameStatistics()
    {
        Console.WriteLine("\nFrame Statistics:");
        Console.WriteLine($"{player.Name}'s frames:");
        foreach (var frame in playerScore.GetFrameScores())
        {
            Console.WriteLine($"Frame {frame.frameNumber}: {frame.firstThrow} + {frame.secondThrow} = {frame.frameTotal}");
        }

        Console.WriteLine($"\n{computerPlayer.Name}'s frames:");
        foreach (var frame in computerScore.GetFrameScores())
        {
            Console.WriteLine($"Frame {frame.frameNumber}: {frame.firstThrow} + {frame.secondThrow} = {frame.frameTotal}");
        }
    }

    private (string winner, int playerTotal, int computerTotal) AnalyzeScores(Score playerScore, Score computerScore)
    {
        int playerTotal = playerScore.GetTotalScore();
        int computerTotal = computerScore.GetTotalScore();

        string winner = playerTotal > computerTotal
            ? player.Name
            : playerTotal < computerTotal
                ? "Computer"
                : "Tie";

        return (winner, playerTotal, computerTotal);
    }
}