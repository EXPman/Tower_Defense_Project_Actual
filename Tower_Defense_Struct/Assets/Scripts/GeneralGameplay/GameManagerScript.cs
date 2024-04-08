using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerScript : MonoBehaviour
{
    [SerializeField] GameObject GameTilePrefab;

    public int XMap = 20;
    public int YMap = 10;

    private void Awake()
    {
        for (int x = 0; x < XMap; x++)
        {
            for (int y = 0; y < YMap; y++)
            {
                Instantiate(GameTilePrefab, new Vector3(x, y, 0), Quaternion.identity);
            }
        }
    }

}
