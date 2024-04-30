using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NewBehaviourScript : MonoBehaviour
{
    public static int TempWaveIndex = 1;
    public static int TempEXP = 10;

    public void LoadSkillTreeScene()
    {
        TempWaveIndex = EnemyWave.Singleton.currentWave;
        TempEXP = EXPScript.EXP;
        SceneManager.LoadScene("SkillTreeScene");
    }

    public void LoadLevel1Scene()
    {
        EnemyWave.Singleton.currentWave = TempWaveIndex;
        EXPScript.EXP = TempEXP; 
        SceneManager.LoadScene("Level1");
    }
}
