using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseCanvas;
    
    [SerializeField]
    private bool isPaused;

    void Start()
    {
        //Hide the UI since the game is not paused by default
        PauseGame(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                PauseGame(false);
            }
            else
            {
                PauseGame(true);
            }
        }
    }
    
    public void PauseGame(bool paused)
    {
        if (paused)
        { 
            //Show the pause menu
           pauseCanvas.SetActive(true);
        }
        else
        {
            //Hide the pause menu
           pauseCanvas.SetActive(false);
        }
        
        isPaused = paused;
        
        //Sets the simulation speed of the game. When 0 time is stopped, when 1 time moves at regular speed
        //https://docs.unity3d.com/ScriptReference/Time-timeScale.html
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
