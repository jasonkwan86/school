using TMPro;
using UnityEngine;

public class UiController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textBox;
    
    public static float secondsPassedSinceStart;

    private void Start()
    {
        secondsPassedSinceStart = 0;
    }

    private void Update()
    {
        if (PauseMenu.IsPaused) return;

        secondsPassedSinceStart += Time.deltaTime;
        textBox.text = "Seconds elapsed: " + secondsPassedSinceStart.ToString("F2");
    }
}
