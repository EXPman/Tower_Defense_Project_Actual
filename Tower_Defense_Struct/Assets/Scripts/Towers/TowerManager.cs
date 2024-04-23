using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerManager : MonoBehaviour
{
    public GameObject[] towerPrefebs;
    private GameObject selectedTowerPrefab;

    public void SelectTower(int index)
    {
        if(index >= 0 && index < towerPrefebs.Length)
        {
            selectedTowerPrefab = towerPrefebs[index];
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
