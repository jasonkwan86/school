using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] GameObject pauseCanvas;
    
    public static bool IsPaused;

    void Start()
    {
        PauseGame(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PauseGame(!IsPaused);
        }
    }
    
    public void PauseGame(bool paused)
    {
        pauseCanvas.SetActive(paused);
        IsPaused = paused;
        
        Time.timeScale = paused ? 0 : 1;
    }

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
