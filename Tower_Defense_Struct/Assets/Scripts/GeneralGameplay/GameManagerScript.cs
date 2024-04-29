using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class GameManagerScript : MonoBehaviour
{
    public static GameManagerScript Instance;

    private EnemyWave enemyWave;

    [SerializeField] GameObject GameTilePrefab;
    [SerializeField] GameObject EnemyPrefab;
    [SerializeField] TMP_Text GoldText;
    private GameTileScript spawnTile;
    GameTileScript[,] gameTiles;
    public int XMap = 20;
    public int YMap = 10;
    bool PathAcctive = false;

    [SerializeField] private Button turretAButton;
    [SerializeField] private Button turretBButton;
    [SerializeField] private Button turretCButton;
    [SerializeField] private Button turretDButton;
    [SerializeField] private Button turretEButton;
    [SerializeField] private TMP_Text Aprice;
    [SerializeField] private TMP_Text Bprice;
    [SerializeField] private TMP_Text Cprice;
    [SerializeField] private TMP_Text Dprice;
    [SerializeField] private TMP_Text Eprice;
    int ACost = GameTileScript.TurretACost;
    int BCost = GameTileScript.TurretBCost;
    int CCost = GameTileScript.TurretCCost;
    int DCost = GameTileScript.TurretDCost;
    int ECost = GameTileScript.TurretECost;

    [SerializeField] public static int gold = 100;

    public GameTileScript TargetTile { get; internal set; }
    List<GameTileScript> pathToGoal = new List<GameTileScript>();

    public static bool IsGamePaused;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        gameTiles = new GameTileScript[XMap, YMap];

        //switch (LevelManager.Singleton.LevelIndex)
        switch(1)
        {
            case 1:
                LoadLevel1();
                break;
            case 2:
                LoadLevel2();
                break;
            case 3:
                LoadLevel3();
                break;
            case 4:
                LoadLevel4();
                break;
            case 5:
                LoadLevel5();
                break;
            default:
                //LoadLevel1();
                LoadLevel2();
                break;
        }
    }

    private void Start()
    {
       enemyWave = FindObjectOfType<EnemyWave>();   
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
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
                //while (TargetTile != null)
                //{
                //    pathToGoal.Add(TargetTile);
                //    TargetTile.SetPath(true);
                //    TargetTile = path[TargetTile];
                //}
                //int enemiesToSpawn = enemyWave.enemiesPerWave();
                int enemiesToSpawn = 5;
                StartCoroutine(SpawnEnemyCoroutine(enemiesToSpawn));

                PathAcctive = true;
            }
        }

        // Mettre à jour l'état du bouton de la tour A
        if (gold < ACost)
        {
            turretAButton.GetComponent<Image>().color = Color.gray; // Bouton devient gris
            Aprice.color = Color.red;
            turretAButton.interactable = false; // Rendre le bouton non interactif
        }
        else
        {
            turretAButton.GetComponent<Image>().color = Color.cyan; // Bouton devient bleu
            Aprice.color = Color.white;
            turretAButton.interactable = true; // Rendre le bouton interactif
        }

        // Mettre à jour l'état du bouton de la tour B
        if (gold < BCost)
        {
            turretBButton.GetComponent<Image>().color = Color.gray; // Bouton devient gris
            Bprice.color = Color.red;
            turretBButton.interactable = false; // Rendre le bouton non interactif
        }
        else
        {
            turretBButton.GetComponent<Image>().color = Color.cyan; // Bouton devient bleu
            Bprice.color = Color.white;
            turretBButton.interactable = true; // Rendre le bouton interactif
        }

        if (gold < CCost)
        {
            turretCButton.GetComponent<Image>().color = Color.gray;
            Cprice.color = Color.red;
            turretCButton.interactable = false;
        }
        else
        {
            turretCButton.GetComponent<Image>().color = Color.cyan;
            Cprice.color = Color.white;
            turretCButton.interactable = true;
        }

        if (gold < DCost)
        {
            turretDButton.GetComponent<Image>().color = Color.gray;
            Dprice.color = Color.red;
            turretDButton.interactable = false;
        }
        else
        {
            turretDButton.GetComponent<Image>().color = Color.cyan;
            Dprice.color = Color.white;
            turretDButton.interactable = true;
        }

        if (gold < ECost)
        {
            turretEButton.GetComponent<Image>().color = Color.gray;
            Eprice.color = Color.red;
            turretEButton.interactable = false;
        }
        else
        {
            turretEButton.GetComponent<Image>().color = Color.cyan;
            Eprice.color = Color.white;
            turretEButton.interactable = true;
        }
        GoldText.text = $"Gold: {gold}";

    }

    public void UpdateGold(int amount)
    {
        gold += amount;
        turretAButton.interactable = gold >= ACost;
        turretBButton.interactable = gold >= BCost;
        turretCButton.interactable = gold >= CCost;
        turretDButton.interactable = gold >= DCost;
        turretEButton.interactable = gold >= ECost;
        GoldText.text = $"Gold: {gold}";
    }


    public void CalculateNewPath() //C<est pour solve un bug 
    {
        if (TargetTile != null)
        {
            bool needsRecalculation = false;

            foreach (var t in gameTiles)
            {
                if (t.IsBlocked && pathToGoal.Contains(t))
                {
                    needsRecalculation = true;
                    break;
                }
            }

            if (needsRecalculation)
            {
                var path = PathFinding(spawnTile, TargetTile);
                var tile = TargetTile;

                pathToGoal.Clear();
                while (tile != null)
                {
                    if (!tile.IsBlocked) // Assurez-vous que la tuile n'est pas bloquée
                    {
                        pathToGoal.Add(tile);
                        tile.SetPath(true);
                    }
                    else
                    {
                        tile.SetPath(false);
                    }
                    tile = path[tile];
                }

                // Informez tous les ennemis qu'un nouveau chemin est disponible
                foreach (var enemy in FindObjectsOfType<Enemy>())
                {
                    enemy.SetPath(pathToGoal);
                }
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

    public IEnumerator SpawnEnemyCoroutine(int numberOfEnemies)
    {
        while(true)
        {
            int enemiesSpawned = 0;
            while (!HP_Script.IsGameOver)
            {
                if (HP_Script.IsGameOver)
                    yield break;

                yield return new WaitForSeconds(0.5f);
                var enemy = Instantiate(EnemyPrefab, spawnTile.transform.position, Quaternion.identity).GetComponent<Enemy>();
                enemy.TargetTile = TargetTile.transform;
                enemy.SetPath(pathToGoal);
                enemiesSpawned++;


                if (enemiesSpawned >= numberOfEnemies)
                     break; // Stop spawning if the wave's enemy count is reached
            }
            yield return new WaitForSeconds(3);
        }
    }

    public void TriggerEnemyWave(int numberOfEnemies)
    {
        StartCoroutine(SpawnEnemyCoroutine(numberOfEnemies));
    }

    public void LoadMap()
    {
        for (int x = 0; x < XMap; x++)
        {
            for (int y = 0; y < YMap; y++)
            {
                var spawnPosition = new Vector3(x - 6, y, 0);
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
    }

    public void LoadLevel1()
    {
        LoadMap();
        spawnTile = gameTiles[0, 4];
        spawnTile.SetEnemySpawn();
        TargetTile = gameTiles[11, 1];
        for (int y = 2; y <= 9; y++)
        {
            gameTiles[5, y].SetWall();
        }

        for (int y = 0; y <= 7; y++)
        {
            gameTiles[10, y].SetWall();
        }
    }

    public void LoadLevel2()
    {
        LoadMap();
        spawnTile = gameTiles[0, 1];
        spawnTile.SetEnemySpawn();
        TargetTile = gameTiles[19, 1];
        for (int x = 0; x <= 6; x++)
        {
            gameTiles[x, 2].SetWall();
        }

        for (int y = 0; y <= 7; y++)
        {
            gameTiles[10, y].SetWall();
        }

        for (int y = 2; y <= 7; y++)
        {
            gameTiles[7, y].SetWall();
        }

        for (int y = 0; y <= 5; y++)
        {
            gameTiles[9, y].SetWall();
        }
        for (int y = 0; y <= 5; y++)
        {
            gameTiles[11, y].SetWall();
        }
        for (int x = 13; x <= 19; x++)
        {
            gameTiles[x, 2].SetWall();
        }

        for (int y = 7; y <= 8; y++)
        {
            gameTiles[8, y].SetWall();
        }

        for (int y = 7; y <= 8; y++)
        {
            gameTiles[12, y].SetWall();
        }

        

        for (int y = 2; y <= 7; y++)
        {
            gameTiles[13, y].SetWall();
        }

        for (int x = 8; x <= 12; x++)
        {
            gameTiles[x, 9].SetWall();
        }
    }

    public void LoadLevel3()
    {
        LoadMap();
        spawnTile = gameTiles[0, 5];
        spawnTile.SetEnemySpawn();
        TargetTile = gameTiles[19, 5];
        for (int x = 0; x <= 4; x++)
        {
            gameTiles[x, 4].SetWall();
        }

        for (int y = 0; y <= 6; y++)
        {
            gameTiles[5, y].SetWall();
        }

        gameTiles[1, 5].SetWall();

        for (int y = 6; y <= 9; y++)
        {
            gameTiles[3, y].SetWall();
        }

        for (int y = 3; y <= 9; y++)
        {
            gameTiles[7, y].SetWall();
        }

        for (int y = 0; y <= 6; y++)
        {
            gameTiles[9, y].SetWall();
        }

        for (int y = 2; y <= 9; y++)
        {
            gameTiles[11, y].SetWall();
        }

        for (int y = 0; y <= 5; y++)
        {
            gameTiles[13, y].SetWall();
        }

        gameTiles[14, 5].SetWall();

        for (int y = 2; y <= 5; y++)
        {
            gameTiles[15, y].SetWall();
        }

        for (int y = 4; y <= 9; y++)
        {
            gameTiles[17, y].SetWall();
        }
    }

    public void LoadLevel4()
    {
        LoadMap();
        spawnTile = gameTiles[5, 9];
        spawnTile.SetEnemySpawn();
        TargetTile = gameTiles[7, 9];
        for (int y = 6; y <= 9; y++)
        {
            gameTiles[6, y].SetWall();
        }

        for (int y = 7; y <= 9; y++)
        {
            gameTiles[4, y].SetWall();
        }

        for (int y = 0; y <= 9; y++)
        {
            gameTiles[3, y].SetWall();
        }

        for (int y = 2; y <= 4; y++)
        {
            gameTiles[5, y].SetWall();
        }

        for (int x = 5; x <= 16; x++)
        {
            gameTiles[x, 5].SetWall();
        }

        for (int x = 5; x <= 17; x++)
        {
            gameTiles[x, 1].SetWall();
        }

        for (int y = 2; y <= 6; y++)
        {
            gameTiles[17, y].SetWall();
        }

        for (int x = 8; x <= 15; x++)
        {
            gameTiles[x, 7].SetWall();
        }

        for (int y = 8; y <= 9; y++)
        {
            gameTiles[15, y].SetWall();
        }
    }

    public void LoadLevel5()
    {
        LoadMap();
        spawnTile = gameTiles[0, 9];
        spawnTile.SetEnemySpawn();
        TargetTile = gameTiles[9, 0];
        for (int x = 1; x <= 19; x++)
        {
            gameTiles[x, 9].SetWall();
        }

        for (int x = 0; x <= 18; x++)
        {
            gameTiles[x, 7].SetWall();
        }

        for (int x = 1; x <= 19; x++)
        {
            gameTiles[x, 5].SetWall();
        }

        for (int x = 0; x <= 18; x++)
        {
            gameTiles[x, 3].SetWall();
        }

        for (int x = 1; x <= 19; x++)
        {
            gameTiles[x, 1].SetWall();
        }
    }
}
