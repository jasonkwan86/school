using TMPro;
using UnityEngine;

public class Leaderboard : MonoBehaviour
{
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private Transform leaderboardListingsHolder;
    [SerializeField] private TMP_Text leaderboardListingPrefab;

    private void Start()
    {
        for (int i = 0; i < leaderboardListingsHolder.childCount; i++)
        {
            Destroy(leaderboardListingsHolder.GetChild(i).gameObject);
        }
        LeaderboardEntries leaderboardEntries = LeaderboardUtils.GetLeaderboardEntries();
        if (leaderboardEntries == null || leaderboardEntries.entries.Count == 0)
        {
            TMP_Text leaderboardListing = Instantiate(leaderboardListingPrefab, leaderboardListingsHolder.transform);
            return;
        }

        foreach (LeaderboardEntry entry in leaderboardEntries.entries)
        {
            TMP_Text leaderboardListing = Instantiate(leaderboardListingPrefab, leaderboardListingsHolder.transform);
            leaderboardListing.text = entry.ToString();
        }
    }

    public void BackToMainMenu()
    {
        mainMenu.SetActive(true);
        gameObject.SetActive(false);
    }
}
