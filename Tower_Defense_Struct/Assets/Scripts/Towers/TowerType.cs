using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TowerType : MonoBehaviour
{
    public static TowerType Instance;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    public void SetType(TowerType type)
    {
        switch(type)
        {
           
        }
    }
}
