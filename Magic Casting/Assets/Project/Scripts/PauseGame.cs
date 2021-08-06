using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseGame : MonoBehaviour
{

    public static bool GameIsPaused = false;

    public GameObject pauseMenuUI;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameIsPaused)
            {
                Resume();             
            }
            else
            {
                Pause();
            }
        }

        else if (Input.GetKeyDown(KeyCode.M))
        {
            if (GameIsPaused)
            {
                SceneManager.LoadScene("Menu");
            }
        }

        else if (Input.GetKeyDown(KeyCode.Q))
        {
            if (GameIsPaused)
            {
                Application.Quit();
            }
        }
    }

    void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
    }

    void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;
    }

    public void LoadMenu()
    {
        if(Input.GetKeyDown(KeyCode.M)){

            SceneManager.LoadScene("Menu");
        }
    }

    public void QuitGame()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            Application.Quit();
        }
    }


} 
