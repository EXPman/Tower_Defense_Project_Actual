using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

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
        gameTiles = new GameTileScript[XMap, YMap];

        for (int x = 0; x < XMap; x++)
        {
            for (int y = 0; y < YMap; y++)
            {
                var spawnPosition = new Vector3(x-6, y, 0);
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
            if (!PathAcctive)
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

        if(gold < CCost)
        {
            turretCButton.GetComponent<Image>().color = Color.gray;
            Cprice.color = Color.red;
            turretCButton.interactable= false;
        }
        else 
        {
            turretCButton.GetComponent <Image>().color = Color.cyan;
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
            turretDButton.GetComponent <Image>().color= Color.cyan;
            Dprice.color = Color.white;
            turretDButton.interactable = true;
        }

        if(gold < ECost)
        {
            turretEButton.GetComponent<Image>().color = Color.gray;
            Eprice.color = Color.red;
            turretEButton.interactable = false; 
        }
        else
        {
            turretEButton.GetComponent<Image>().color= Color.cyan;
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
