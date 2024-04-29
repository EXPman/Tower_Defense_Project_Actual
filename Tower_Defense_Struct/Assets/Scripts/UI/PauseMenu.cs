using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject PausePanel;

    public static bool isPaused;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                Continue();
            }
            else
            {
                Pause();
            }
        }

    }

    public void Pause()
    {
        isPaused = true;
        PausePanel.SetActive(true);
        Time.timeScale = 0f;
    }

    public void Continue()
    {
        if(!HP_Script.IsGameOver)
        {
            isPaused = false;
            PausePanel.SetActive(false);
            Time.timeScale = 1f;
        }
    }
    public void Restart()
    {
        GameManagerScript.gold = 100;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void QuitGame()
    {
        Debug.Log("Quitting game...");

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false; // Quitte le mode de lecture dans l'éditeur Unity
#else
    Application.Quit(); // Quitte l'application
#endif
    }


}
