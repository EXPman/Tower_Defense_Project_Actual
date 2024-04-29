using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public byte LevelIndex = 1;

    public static LevelManager Singleton;

    private void Awake()
    {
        if (Singleton == null)
        {
            Singleton = this;
        }
        else
        {
            Destroy(this.gameObject);
        }

        DontDestroyOnLoad(this);
    }

    public void OnClick1()
    {
        LevelIndex = 1;
    }

    public void OnClick2()
    {
        LevelIndex = 2;
    }

    public void OnClick3()
    {
        LevelIndex = 3;
    }

    public void OnClick4()
    {
        LevelIndex = 4;
    }

    public void OnClick5()
    {
        LevelIndex = 5;
    }
}
