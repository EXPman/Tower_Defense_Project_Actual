using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EXPTextForSkillTree : MonoBehaviour
{
    [SerializeField] TMP_Text EXPText;


    private void Start()
    {
        EXPText.text = $"EXP: {EXPScript.EXP}";
    }

    public void ChangeEXP()
    {
        EXPText.text = $"EXP: {EXPScript.EXP}";
    }
}
