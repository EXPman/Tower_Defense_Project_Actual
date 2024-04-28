using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Events;

public class EnemyWave : MonoBehaviour
{
    [SerializeField] private GameObject spawner;
    [SerializeField] private Sprite[] enemyPrefabs;
    [SerializeField] private int totalWaves = 5;
    [SerializeField] private int baseEnemies = 6;
    [SerializeField] private float difficultyScaling = 0.75f;
    [SerializeField] private float timeBetweenWaves = 5f;
    [SerializeField] private float enemiesPerSecond = 0.5f;

    public static UnityEvent onEnemyDestroy;

    private int currentWave = 1;
    private float timeSinceLastSpawn;
    private int enemiesAlive;
    private int enemiesTotal;
    private bool isSpawning = false;

    private void Awake()
    {
        onEnemyDestroy.AddListener(EnemyDestroyed);
    }

    private void Start()
    {
        StartCoroutine(StartWave());
    }

    private void Update()
    {
        if (!isSpawning) return;

        timeSinceLastSpawn += Time.deltaTime; 

        if(timeSinceLastSpawn >= (1f / enemiesPerSecond) && enemiesTotal > 0)
        {
            Debug.Log("Spawn enemies");
            FindObjectOfType<GameManagerScript>().SpawnEnemyCoroutine();
            enemiesTotal--;
            enemiesAlive++;
            timeSinceLastSpawn = 0f;
        }

        if(enemiesAlive == 0 && enemiesTotal == 0)
        {
            EndWave();
        }
    }

    private IEnumerator StartWave()
    {
        yield return new  WaitForSeconds(timeBetweenWaves); 

        isSpawning = true;
        enemiesTotal = enemiesPerWave();
    }

    private void EnemyDestroyed()
    {
        enemiesAlive--;
    }

    private void EndWave()
    {
        isSpawning = false;
        timeSinceLastSpawn = 0f;
        currentWave++; 
        
    }

    private int enemiesPerWave()
    {
        return Mathf.RoundToInt(baseEnemies * Mathf.Pow(currentWave, difficultyScaling));
    }
}
