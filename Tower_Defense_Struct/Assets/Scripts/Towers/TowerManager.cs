using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerManager : MonoBehaviour
{

    public static TowerManager Singleton;


    public int NumberOfTowers = 5;
    public int CurrentTowerIndex = -1;


    void Awake()
    {
        //makes sure the script is singleton
        if (Singleton == null)
        {
            Singleton = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    public void SelectTower(int index)
    {
        if(index >= 0 && index < NumberOfTowers)
        {
            CurrentTowerIndex = index;
        }
    }

    public void OnTurrerAButtonClicked()
    {
        if(GameManagerScript.gold >= GameTileScript.TurretACost)
        {
            CurrentTowerIndex = 0;
        }
        else
        {
            Debug.Log("Pas assez d'or pour la tourelle A.");
        }
    }

    public void OnTurretBButtonClicked()
    {
        if (GameManagerScript.gold >= GameTileScript.TurretBCost)
        {
            CurrentTowerIndex = 1; // Supposons que towerPrefabs[1] est la tour B.
        }
        else
        {
            Debug.Log("Pas assez d'or pour la tourelle B.");
        }
    }

    public void OnTurrerCButtonClicked()
    {
        if (GameManagerScript.gold >= GameTileScript.TurretCCost)
        {
            CurrentTowerIndex = 2;
        }
        else
        {
            Debug.Log("Pas assez d'or pour la tourelle C.");
        }
    }

    public void OnTurretDButtonClicked()
    {
        if (GameManagerScript.gold >= GameTileScript.TurretDCost)
        {
            CurrentTowerIndex = 3; // Supposons que towerPrefabs[1] est la tour B.
        }
        else
        {
            Debug.Log("Pas assez d'or pour la tourelle D.");
        }
    }
    public void OnTurrerEButtonClicked()
    {
        if (GameManagerScript.gold >= GameTileScript.TurretECost)
        {
            CurrentTowerIndex = 4;
        }
        else
        {
            Debug.Log("Pas assez d'or pour la tourelle E.");
        }
    }

    public int GetSelectedTower()
    {
        return CurrentTowerIndex;
    }

    public bool CanPlaceTowerHere()
    {
        return CurrentTowerIndex != -1;
    }
}
