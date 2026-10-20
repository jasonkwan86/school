using System.Collections.Generic;
using UnityEngine;

public class LeaderboardUtils
{
    private static readonly string _leaderboard_key = "leaderboard";
    
    public static void AddToLeaderboard(LeaderboardEntry newEntry)
    {
        LeaderboardEntries fetchedEntries = GetLeaderboardEntries();
        fetchedEntries.entries.Add(newEntry);
        string stringifiedEntries = JsonUtility.ToJson(fetchedEntries);
        
        PlayerPrefs.SetString(_leaderboard_key, stringifiedEntries);
        PlayerPrefs.Save();
    }
    
    public static LeaderboardEntries GetLeaderboardEntries()
    {
        if (!PlayerPrefs.HasKey(_leaderboard_key))
            return new LeaderboardEntries();

        string json = PlayerPrefs.GetString(_leaderboard_key);

        return JsonUtility.FromJson<LeaderboardEntries>(json);
    }
}

[System.Serializable]
public class LeaderboardEntry
{
    public float secondsToComplete;
    public string name;
    
    public LeaderboardEntry(float secondsToComplete, string name)
    {
        this.secondsToComplete = secondsToComplete;
        this.name = name;
    }

    public override string ToString()
    {
        return secondsToComplete + " by: " +  name;
    }
}

public class LeaderboardEntries
{
    public List<LeaderboardEntry> entries;

    public LeaderboardEntries()
    {
        entries = new List<LeaderboardEntry>();
    }
}