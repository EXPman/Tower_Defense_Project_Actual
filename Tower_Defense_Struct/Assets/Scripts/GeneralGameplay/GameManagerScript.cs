using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class GameManagerScript : MonoBehaviour
{
    [SerializeField] GameObject GameTilePrefab;
    [SerializeField] GameObject EnemyPrefab;
    [SerializeField] TMP_Text GoldText;
    private GameTileScript spawnTile;
    GameTileScript[,] gameTiles;
    public int XMap = 20;
    public int YMap = 10;
    bool PathAcctive = false;
    


    [SerializeField] public static int gold = 100;

    public GameTileScript TargetTile { get; internal set; }
    List<GameTileScript> pathToGoal = new List<GameTileScript>();

    public static bool IsGamePaused;

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
        spawnTile = gameTiles[0, 4];
        spawnTile.SetEnemySpawn();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && TargetTile != null)
        {
            if(!PathAcctive)
            {
                    foreach (var T in gameTiles)
                    {
                        T.SetPath(false);
                    }

                    var path = PathFinding(spawnTile, TargetTile);
                    var tile = TargetTile;

                    pathToGoal.Clear();
                    while (tile != null)
                    {
                        pathToGoal.Add(tile);
                        tile.SetPath(true);
                        tile = path[tile];
                    }
                    StartCoroutine(SpawnEnemyCoroutine());

                    PathAcctive = true;
                
            }
        }

        GoldText.text = $"Gold: {gold}";

    }

    public void CalculateNewPath() //C<est pour solve un bug 
    {
        if (TargetTile != null)
        {
            foreach (var t in gameTiles)
            {
                t.SetPath(false);
            }

            var path = PathFinding(spawnTile, TargetTile);
            var tile = TargetTile;

            pathToGoal.Clear();
            while (tile != null)
            {
                pathToGoal.Add(tile);
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
        while (!HP_Script.IsGameOver)
        {
            for (int q = 0; q < 5; q++) 
            {
                if (HP_Script.IsGameOver)  // Vérifie à nouveau ici avant de commencer à créer les ennemis
                    yield break;
                for (int i = 0; i < 5; i++)
                {
                    if (HP_Script.IsGameOver) //vérifie avant chaque ennemi
                        yield break;

                    yield return new WaitForSeconds(0.5f);
                    var enemy = Instantiate(EnemyPrefab, spawnTile.transform.position, Quaternion.identity).GetComponent<Enemy>();
                    enemy.SetPath(pathToGoal);
                }
                yield return new WaitForSeconds((2f));
            }
        }
    }

}
