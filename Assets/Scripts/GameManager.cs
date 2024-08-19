using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : SingletonClass<GameManager>
{

    private void Start()
    {
        Time.timeScale = 1;
    }

    public bool gamePaused = false;
    public GameObject pausedMenu;

    public void PauseGame()
    {
        gamePaused = true;
        pausedMenu.SetActive(true);
        Time.timeScale = 0;
    }

    public void ResumeGame()
    {
        gamePaused = false;
        pausedMenu.SetActive(false);
        Time.timeScale = 1;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (gamePaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    public void Restart()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
        Time.timeScale = 1;
    }
}
