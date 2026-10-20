using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class DeletePlayerPrefs : MonoBehaviour
{
    [MenuItem("Tools/Clear PlayerPrefs")]
    private static void NewMenuOption()
    {
        PlayerPrefs.DeleteAll();
        Debug.Log("Player Prefs cleared.");
    }
    
    [MenuItem("Tools/Set Mock PlayerPrefs")]
    private static void SetMockPlayerPrefsMenuOption()
    {
        List<LeaderboardEntry> fetchedEntries = new List<LeaderboardEntry>()
        {
            new LeaderboardEntry(1.01f, "FAST"),
            new LeaderboardEntry(15, "BAM"),
            new LeaderboardEntry(30, "BAP"),
            new LeaderboardEntry(100, "BAS"),
            new LeaderboardEntry(1000.02f, "SLOW"),
        };

        foreach (LeaderboardEntry entry in fetchedEntries)
        {
            LeaderboardUtils.AddToLeaderboard(entry);
        }

        Debug.Log("Set mock player preferences");
    }
    
    [MenuItem("Tools/Print PlayerPrefs")]
    private static void PrintPlayerPrefsMenuOption()
    {
        LeaderboardEntries leaderboardEntries = LeaderboardUtils.GetLeaderboardEntries();
        Debug.Log("Entries of size: " + leaderboardEntries.entries);
        foreach (LeaderboardEntry entry in leaderboardEntries.entries)
        {
            Debug.Log(entry);
        }
    }
}
