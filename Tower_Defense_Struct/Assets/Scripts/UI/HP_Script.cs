using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class HP_Script : MonoBehaviour
{
    [SerializeField] TMP_Text HPtext;
    public static int HPvalue = 50;
    public static int BonusHP;

    public static bool IsGameOver = false;
    public static event Action OnGameOver; 

    private void Awake()
    {
        HPtext.text = $"HP: {HPvalue + BonusHP}";
        Enemy.OnEnemyReachedEnd += HandleEnemyReachedEnd;
    }

    private void OnDestroy()
    {
        Enemy.OnEnemyReachedEnd -= HandleEnemyReachedEnd;
    }

    private void HandleEnemyReachedEnd()
    {
        if (IsGameOver) return;

        HPvalue--;
        HPtext.text = $"HP: {HPvalue + BonusHP}";

        if(HPvalue <= 0)
        {
            HPvalue = 0;
            if (!IsGameOver)
            {
                IsGameOver = true;
                OnGameOver();
                EndGame();
            }
        }
       
    }

    private void EndGame()
    {
        StopAllCoroutines();
        Debug.Log("Game Over");

    }
}
