using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameManagerScript : MonoBehaviour
{
    [SerializeField] GameObject GameTilePrefab;
    [SerializeField] GameObject EnemyPrefab;
    private GameTileScript spawnTile;
    GameTileScript[,] gameTiles;
    public int XMap = 20;
    public int YMap = 10;

    public GameTileScript TargetTile { get; internal set; }

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
                gameTiles[x, y].GM = this;
                gameTiles[x, y].X = x;
                gameTiles[x, y].Y = y;

                if ((x + y) % 2 == 0)
                {
                    gameTiles[x, y].TurnGrey();
                }
            }
        }
        spawnTile = gameTiles[1, 4];
        spawnTile.SetEnemySpawn();
        //StartCoroutine(SpawnEnemyCoroutine());
    }

    private void Update()
    {
        if (Input.GetKeyDown((KeyCode.Space)) && TargetTile != null)
        {
            foreach(var t in gameTiles)
            {
                t.SetPath(false);
            }

            var path = PathFinding(spawnTile, TargetTile);
            var tile = TargetTile; 

            while(tile != null)
            {
                tile.SetPath(true);
                tile = path[tile];
            }
        }
    }

    private Dictionary<GameTileScript, GameTileScript> PathFinding(GameTileScript sourceTile, GameTileScript targetTile)
    {
        var dist = new Dictionary<GameTileScript, int>();
        var prev = new Dictionary<GameTileScript, GameTileScript>();
        var Q = new List<GameTileScript>();

        foreach (var v in gameTiles)
        {
            dist.Add(v, 9999);

            prev.Add(v, null);

            Q.Add(v);
        }

        dist[sourceTile] = 0;

        while (Q.Count > 0)
        {
            GameTileScript u = null;
            int minDistance = int.MaxValue;

            foreach (var v in Q)
            {
                if ((dist[v] < minDistance))
                {
                    minDistance = dist[v];
                    u = v;
                }
            }

            Q.Remove(u);

            foreach (var v in FindNeighbor(u))
            {
                if (!Q.Contains(v) || v.IsBlocked)
                {
                    continue;
                }

                int alt = dist[u] + 1;

                if (alt < dist[v])
                {
                    dist[v] = alt;

                    prev[v] = u;
                }

            }
        }

        return prev;
    }

    private List<GameTileScript> FindNeighbor(GameTileScript u)
    {
        var result = new List<GameTileScript>();
        if (u.X - 1 >= 0)
            result.Add(gameTiles[u.X - 1, u.Y]);
        if (u.X + 1 < XMap)
            result.Add(gameTiles[u.X + 1, u.Y]);
        if (u.Y - 1 >= 0)
            result.Add(gameTiles[u.X, u.Y - 1]);
        if (u.Y + 1 < YMap)
            result.Add(gameTiles[u.X, u.Y + 1]);
        return result;

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
