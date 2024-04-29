using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
   

    public void LoadScene(string sceneName)
    {
        if(sceneName == "Level1")
        {
            GameManagerScript.gold = 100;
        }
        SceneManager.LoadScene(sceneName);
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
