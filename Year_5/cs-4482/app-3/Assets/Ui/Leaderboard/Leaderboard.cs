using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class Leaderboard : MonoBehaviour
{
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private Transform leaderboardListingsHolder;
    [SerializeField] private TMP_Text leaderboardListingPrefab;
    [SerializeField] private int entriesToShow = 3;
    
    private void Start()
    {
        for (int i = 0; i < leaderboardListingsHolder.childCount; i++)
        {
            Destroy(leaderboardListingsHolder.GetChild(i).gameObject);
        }
        LeaderboardEntries leaderboardEntries = LeaderboardUtils.GetLeaderboardEntries();
        if (leaderboardEntries == null || leaderboardEntries.entries.Count == 0)
        {
            // Just make one if there are none
            Instantiate(leaderboardListingPrefab, leaderboardListingsHolder.transform);
            return;
        }

        // If there are already entries then get them and sort them
        List<LeaderboardEntry> sortedEntriesByTime = leaderboardEntries.entries.OrderBy(entry => entry.secondsToComplete).ToList();
        for (int i = 0; i < Math.Min(entriesToShow, sortedEntriesByTime.Count); i++)
        {
            TMP_Text leaderboardListing = Instantiate(leaderboardListingPrefab, leaderboardListingsHolder.transform);
            leaderboardListing.text = sortedEntriesByTime[i].ToString();
        }
    }

    public void BackToMainMenu()
    {
        mainMenu.SetActive(true);
        gameObject.SetActive(false);
    }
}
