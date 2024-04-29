using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Events;

public class EnemyWave : MonoBehaviour
{
    public static EnemyWave Singleton;

    //[SerializeField] private GameObject spawner;
    //[SerializeField] private Sprite[] enemyPrefabs;
    [SerializeField] private int totalWaves = 5;
    [SerializeField] private int baseEnemies = 6;
    [SerializeField] private int difficultyScaling = 1;
    [SerializeField] private float timeBetweenWaves = 5f;
    [SerializeField] private float enemiesPerSecond = 0.5f;

    public static UnityEvent onEnemyDestroy;

    public int currentWave = 1;
    private float timeSinceLastSpawn;
    public int enemiesAlive;
    private int enemiesTotal;
    public static bool isSpawning = false;

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

    private void Start()
    {
        if(LevelManager.Singleton != null)
        {
            difficultyScaling = LevelManager.Singleton.LevelIndex;
        }
        baseEnemies *= difficultyScaling;
        DontDestroyOnLoad(this);
    }

    public int enemiesPerWave()
    {
        // Assurez-vous que la base et l'exposant sont positifs
        if (baseEnemies <= 0 || currentWave <= 0 || difficultyScaling <= 0)
        {
            Debug.LogError("Les valeurs de baseEnemies, currentWave ou difficultyScaling sont négatives ou nulles, ce qui n'est pas prévu.");
            return 0; // Retourne 0 pour éviter la génération d'ennemis dans un état d'erreur
        }

        // Calculer le nombre d'ennemis, en s'assurant qu'il est toujours positif
        //float calculatedEnemies = baseEnemies * currentWave;
        int calculatedEnemies = baseEnemies * currentWave;
        //int enemiesToSpawn = Mathf.Max(1, Mathf.RoundToInt(calculatedEnemies)); // Assurez-vous de toujours générer au moins un ennemi
        //return enemiesToSpawn;
        return calculatedEnemies;
    }
}
