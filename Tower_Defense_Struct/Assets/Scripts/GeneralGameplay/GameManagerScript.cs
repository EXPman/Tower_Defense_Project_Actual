using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerScript : MonoBehaviour
{
    [SerializeField] GameObject GameTilePrefab;
    [SerializeField] GameObject EnemyPrefab;
    private GameTileScript spawnTile;
    GameTileScript[,] gameTiles;
    public int XMap = 20;
    public int YMap = 10;
   

    private void Awake()
    {
       gameTiles = new GameTileScript[XMap, YMap];

        for (int x = 0; x < XMap; x++)
        {
            for (int y = 0; y < YMap; y++)
            {
                var spawnPosition = new Vector3(x, y, 0);
                var tile = Instantiate(GameTilePrefab, spawnPosition, Quaternion.identity);
                gameTiles[x, y] = tile.GetComponent<GameTileScript>();
                if( (x + y) % 2 == 0)
                {
                    gameTiles[x, y].TurnGrey();
                }
            }
        }
        spawnTile = gameTiles[1, 4];
        spawnTile.SetEnemySpawn();
        StartCoroutine(SpawnEnemyCoroutine());
    }
    
    IEnumerator SpawnEnemyCoroutine()
    {
        while (true)
        {
            for (int q = 0; q < 5; q++)
            {
                for (int i = 0; i < 5; i++)
                {
                    Instantiate(EnemyPrefab, spawnTile.transform.position, Quaternion.identity);
                    yield return new WaitForSeconds(0.5f);
                }
                yield return new WaitForSeconds((2f));
            }
      
        }
    }

}
