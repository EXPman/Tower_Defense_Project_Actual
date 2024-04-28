using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerManager : MonoBehaviour
{

    public static TowerManager Singleton;


    public GameObject[] towerPrefebs;
    private GameObject selectedTowerPrefab;


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
        if(index >= 0 && index < towerPrefebs.Length)
        {
            selectedTowerPrefab = towerPrefebs[index];
        }
    }

    public void OnTurrerAButtonClicked()
    {
        if(GameManagerScript.gold >= GameTileScript.TurretACost)
        {
            selectedTowerPrefab = towerPrefebs[0];
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
            selectedTowerPrefab = towerPrefebs[1]; // Supposons que towerPrefabs[1] est la tour B.
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
            selectedTowerPrefab = towerPrefebs[2];
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
            selectedTowerPrefab = towerPrefebs[3]; // Supposons que towerPrefabs[1] est la tour B.
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
            selectedTowerPrefab = towerPrefebs[4];
        }
        else
        {
            Debug.Log("Pas assez d'or pour la tourelle E.");
        }
    }

    public GameObject GetSelectedTower()
    {
        return selectedTowerPrefab;
    }

    public bool CanPlaceTowerHere()
    {
        return selectedTowerPrefab != null;
    }
}
