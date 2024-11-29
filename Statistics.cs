﻿using System;
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

public class PlayerFrequency : IGameData
{
    public string PlayerName { get; set; }
}

public class BowlingStatistics<T> where T : IGameData
{
    private List<T> statistics = new();
    private readonly string filePath;

    public BowlingStatistics()
    {
        filePath = typeof(T) == typeof(GameHistory)
            ? "gamehistory.json"
            : "playerfrequency.json";

        LoadStatistics();
    }

    // KRAV #:
    // 1: Generics
    // 2: Konceptet används genom en generisk klass 
    // 3: Ovanför används Generics för att kunna ta in statistik oavsett vilken datatyp som statistiken består av
    // den generiska klassen BowlingStatistics har en constraint som är IGameData 

    private void LoadStatistics()
    {
        if (File.Exists(filePath))
        {
            string jsonData = File.ReadAllText(filePath);
            statistics = JsonSerializer.Deserialize<List<T>>(jsonData) ?? new List<T>();
        }
    }
    
    //Här har vi tagit hjälp av LLM för att få insperation och repetition av hur JSON filer används och seraliseras
    private void SaveStatistics()                       
    {
        var options = new JsonSerializerOptions { WriteIndented = true };
        string jsonData = JsonSerializer.Serialize(statistics, options);
        File.WriteAllText(filePath, jsonData);
    }

    public void AddData(T data)
    {
        if (typeof(T) == typeof(GameHistory))
        {
            statistics.Add(data);
            statistics = statistics.OrderByDescending(s => (s as GameHistory).Score).Take(3).ToList();
        }
        else if (typeof(T) == typeof(PlayerFrequency))
        {
            statistics.Add(data);
        }
        SaveStatistics();
    }

    // KRAV #:
    // 1: LINQ
    // 2: LINQ används genom implementationen OrderByDescending samt COUNT
    // 3: Vi använder LINQ för att skapa en high score lista, där vi tar fram topp 3 samt antalet gånger en spelare har spelat. 
    //Detta blir menginsfullt eftersom att koden blir mer simpel än om vi inte hade använt LINQ


    public void ShowStatistics(string playerName = "")
    {
        if (typeof(T) == typeof(GameHistory))
        {
            Console.WriteLine("\n TOP 3 SCORES OF ALL TIME ");
            Console.WriteLine("--------------------------------");
            int rank = 1;
            foreach (var score in statistics)
            {
                var history = score as GameHistory;
                Console.WriteLine($"{rank++}. {history.PlayerName}: {history.Score} points");
            }
        }
        else if (typeof(T) == typeof(PlayerFrequency))
        {
            var gamesPlayed = statistics.Count(s => s.PlayerName.Equals(playerName, StringComparison.OrdinalIgnoreCase));
            Console.WriteLine($"\n{playerName} has played {gamesPlayed} games");
        }
    }
}