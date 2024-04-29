using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WinScreen_Script : MonoBehaviour
{
    [SerializeField] public GameObject WinScreen;




    public void ActivateWinScreen()
    {
        WinScreen.SetActive(true);

        Time.timeScale = 0f;
    }

    public void Restart()
    {
        GameManagerScript.gold = 100;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void PressQuit()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
