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
        Debug.Log($"Enemy reached end. Current HP: {HPvalue}");

        if (IsGameOver) return;
        if(HPvalue > 0)
        {
            HPvalue--;
            HPtext.text = $"HP: {HPvalue + BonusHP}";
        }
    
        if(HPvalue <= 0 && !IsGameOver)
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
        StopAllCoroutines(); //Les enemmies ne spawn plus 
        Debug.Log("Game Over");

    }
}
