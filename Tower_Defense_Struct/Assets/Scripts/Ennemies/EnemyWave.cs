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

    private void Start()
    {
        StartCoroutine(StartWave());
        Enemy.OnEnemyDestroyed += EnemyDestroyed;
    }

    private void Update()
    {
        if (!isSpawning) return;

        timeSinceLastSpawn += Time.deltaTime;

        if (timeSinceLastSpawn >= (1f / enemiesPerSecond) && enemiesTotal > 0)
        {
            Debug.Log("Spawn enemies");
            enemiesTotal--;
            enemiesAlive++;
            timeSinceLastSpawn = 0f;
            Debug.Log($"Il reste {enemiesTotal} ennemis à générer dans cette vague.");
        }

        if (enemiesAlive == 0 && enemiesTotal == 0)
        {
            EndWave();
        }
    }

    private IEnumerator StartWave()
    {
        yield return new WaitForSeconds(timeBetweenWaves);
        isSpawning = true;
        enemiesTotal = enemiesPerWave();
        GameManagerScript.Instance.TriggerEnemyWave(enemiesTotal);

    }

    private void EndWave()
    {
        isSpawning = false;
        timeSinceLastSpawn = 0f;
        currentWave++;

    }

    private void OnDestroy()
    {
        Enemy.OnEnemyDestroyed -= EnemyDestroyed;
    }

    private void EnemyDestroyed(Enemy enemy)
    {
        enemiesAlive--;
        Debug.Log($"Il reste {enemiesAlive} ennemis vivants dans cette vague.");

        if (enemiesAlive == 0 && enemiesTotal == 0)
        {
            EndWave();
        }
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
        float calculatedEnemies = baseEnemies * Mathf.Pow(currentWave, difficultyScaling);
        int enemiesToSpawn = Mathf.Max(1, Mathf.RoundToInt(calculatedEnemies)); // Assurez-vous de toujours générer au moins un ennemi
        return enemiesToSpawn;
    }
}
