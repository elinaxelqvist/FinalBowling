using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

public interface IGameData
{
    string PlayerName { get; }
}

public class GameHistory : IGameData
{
    public string PlayerName { get; set; }
    public int Score { get; set; }
}

public class GameStatistics<T> where T : IGameData
{
    private List<T> statistics = new();
    private readonly string filePath;
    private readonly string fileFormat;
    private readonly string gamesPlayedPath;

    public GameStatistics(string format = "json")
    {
        fileFormat = format.ToLower();
        filePath = $"gamehistory.{fileFormat}";
        gamesPlayedPath = "gamesplayed.json";
        LoadStatistics();
    }

    private Dictionary<string, int> LoadGamesPlayed()
    {
        if (!File.Exists(gamesPlayedPath))
            return new Dictionary<string, int>();

        string json = File.ReadAllText(gamesPlayedPath);
        return JsonSerializer.Deserialize<Dictionary<string, int>>(json) ?? new Dictionary<string, int>();
    }

    private void SaveGamesPlayed(Dictionary<string, int> gamesPlayed)
    {
        string json = JsonSerializer.Serialize(gamesPlayed, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(gamesPlayedPath, json);
    }

    private void LoadStatistics()
    {
        if (!File.Exists(filePath))
            return;

        string data = File.ReadAllText(filePath);
        statistics = fileFormat switch
        {
            "json" => JsonSerializer.Deserialize<List<T>>(data) ?? new List<T>(),
            "csv" => ParseCsv(data),
            _ => new List<T>()
        };
    }

    private void SaveStatistics()
    {
        string data = fileFormat switch
        {
            "json" => JsonSerializer.Serialize(statistics, new JsonSerializerOptions { WriteIndented = true }),
            "csv" => ToCsv(),
            _ => string.Empty
        };

        File.WriteAllText(filePath, data);
    }

    private List<T> ParseCsv(string csvData)
    {
        var results = new List<T>();
        var lines = csvData.Split('\n', StringSplitOptions.RemoveEmptyEntries);
        
        foreach (var line in lines.Skip(1)) // Skip header
        {
            var values = line.Split(',');
            if (values.Length >= 2 && typeof(T) == typeof(GameHistory))
            {
                results.Add((T)(IGameData)new GameHistory 
                { 
                    PlayerName = values[0], 
                    Score = int.Parse(values[1]) 
                });
            }
        }
        
        return results;
    }

    private string ToCsv()
    {
        if (!statistics.Any())
            return "PlayerName,Score";

        return "PlayerName,Score\n" + string.Join("\n", 
            statistics
                .Where(s => s is GameHistory)  // Filtrerar först
                .Select(s => $"{s.PlayerName},{((GameHistory)(object)s).Score}"));  // Transformerar sen
    }

    public void AddData(T data)
    {
        if (typeof(T) == typeof(GameHistory))
        {
            statistics.Add(data);
            
            statistics = statistics
                .OrderByDescending(s => ((GameHistory)(object)s).Score)
                .Take(3)
                .AsEnumerable()  // Behåller deferred execution
                .ToList();  // Konverterar till List bara när vi måste spara
            
            SaveStatistics();

            var gamesPlayed = LoadGamesPlayed();
            string playerName = data.PlayerName;
            if (!gamesPlayed.ContainsKey(playerName))
                gamesPlayed[playerName] = 0;
            gamesPlayed[playerName]++;
            SaveGamesPlayed(gamesPlayed);
        }
    }

    public void ShowStatistics()
    {
        if (typeof(T) == typeof(GameHistory))
        {
            Console.WriteLine($"\nTOP 3 SCORES OF ALL TIME");
            Console.WriteLine("------------------------");
            int rank = 1;
            foreach (var score in statistics)
            {
                var history = (GameHistory)(object)score;
                Console.WriteLine($"{rank++}. {history.PlayerName}: {history.Score} points");
            }
        }
    }

    public int GetGamesPlayed(string playerName)
    {
        var gamesPlayed = LoadGamesPlayed();
        return gamesPlayed.TryGetValue(playerName, out int count) ? count : 0;
    }
}