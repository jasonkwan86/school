using UnityEngine;

public class FinishArea : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<Player>())
        {
            GameManager.ExitToLeaderboardSubmission();
        }
    }
}
