using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // Player must respawn at the start point on death
    public static void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    // The game must end when reaching the goal point
    public static void ExitToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public static void ExitToLeaderboardSubmission()
    {
        SceneManager.LoadScene("LeaderboardEntryMenu");
    }
}
