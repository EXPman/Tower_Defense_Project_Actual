using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HP_Script : MonoBehaviour
{
    [SerializeField] TMP_Text HPtext;
    public static int HPvalue = 3;
    public static int BonusHP;

    private void Awake()
    {
        HPtext.text = $"HP: {HPvalue + BonusHP}";
    }


}
