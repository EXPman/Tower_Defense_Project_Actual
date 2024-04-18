using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NewBehaviourScript : MonoBehaviour
{

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Tab))
        {
            SceneManager.LoadScene("InLevelScene");
            //SceneManager.SetActiveScene(SceneManager.GetSceneByName("InLevelScene"));
        }
    }

    public void LoadSkillTreeScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
