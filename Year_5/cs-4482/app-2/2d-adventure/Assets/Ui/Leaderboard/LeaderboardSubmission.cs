using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LeaderboardSubmission : MonoBehaviour
{
    [SerializeField] private TMP_InputField nameEntryField;
    [SerializeField] private Button submitButton;
    [SerializeField] private TMP_Text scoreEntryField;

    private float _timeToComplete;
        
    private void Start()
    {
        submitButton.interactable = false;
        _timeToComplete = UiController.secondsPassedSinceStart;
        scoreEntryField.text = "Score: " + _timeToComplete.ToString("F2") + " seconds";
    }

    public void CalculateSubmissionValidity(string value)
    {
        submitButton.interactable = nameEntryField.text.Length >= 3;
    }
    
    public void SubmitToLeaderboard()
    {
        LeaderboardUtils.AddToLeaderboard(new LeaderboardEntry(1.55f, nameEntryField.text));
        GameManager.ExitToMainMenu();
    }
}

