using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class DungeonRoom : MonoBehaviour
{
    [Header("General Settings")]
    public int roomProbability = 20; // 20% chances of being picked
    [SerializeField] private Transform playerSpawn;

    [Header("Enemy")]
    [SerializeField] private GameObject[] enemyPrefabs = new GameObject[0];
    [SerializeField] private Transform[] enemySpawns = new Transform[0];
    [SerializeField] private int maxEnemyCount = 1;
    [SerializeField] private bool randomizeCount = true;
    [SerializeField] private bool canLeaveImmediately = true;
    [SerializeField] private bool destroyEnemiesOnLeave = false;

    [Header("Interactables")]
    [SerializeField] private GameObject[] interactablePrefabs = new GameObject[0];
    [SerializeField] private Transform[] interactableSpawns = new Transform[0];
    [SerializeField] private int maxInteratableCount = 1;


    List<Enemy> currentEnemies = new List<Enemy>();

    public event Action OnRoomInitialized;

    public void InitializeRoom()
    {
        SpawnEnemies();
        SpawnInteractables();

        OnRoomInitialized?.Invoke();
    }

    public bool CanLeave()
    {
        return canLeaveImmediately || currentEnemies.Count < 1;
    }

    public void CleanRoom()
    {
        if (destroyEnemiesOnLeave)
        {
            // Make a copy to safely iterate
            List<Enemy> enemiesToDestroy = new List<Enemy>(currentEnemies);
            foreach (Enemy currentEnemy in enemiesToDestroy)
            {
                RemoveEnemy(currentEnemy);
                Destroy(currentEnemy.gameObject);
            }
        }
    }

    private void SpawnEnemies()
    {
        if (maxEnemyCount < 1 || enemyPrefabs.Length < 1)
        {
            Debug.Log("[Room] Cant spawn Enemies. (enemyCount is 0, or spawnCount is 0, or prefabCount is 0)");
            return;
        }

        // Create and populate a Vector2 array with spawn positions
        Vector2[] spawnPositions;
        if (enemySpawns != null && enemySpawns.Length > 0)
        {
            spawnPositions = new Vector2[enemySpawns.Length];
            for (int i = 0; i < enemySpawns.Length; i++)
            {
                spawnPositions[i] = enemySpawns[i].position;
            }
        }
        else
        {
            spawnPositions = new Vector2[] { transform.position };
        }

        int count = randomizeCount && maxEnemyCount > 0 ? Random.Range(1, maxEnemyCount) : maxEnemyCount;

        // Spawn enemies
        for (int i = 0; i < count; i++)
        {
            GameObject prefab = enemyPrefabs[Random.Range(0, enemyPrefabs.Length)];
            Vector2 spawnPos = spawnPositions[Random.Range(0, spawnPositions.Length)];
            InstantiateEnemy(prefab, spawnPos);
        }
    }

    private void InstantiateEnemy(GameObject prefab, Vector3 position)
    {
        GameObject instance = Instantiate(prefab, position, Quaternion.identity, transform);
        Enemy enemy = instance.GetComponent<Enemy>();

        if (enemy != null)
        {
            enemy.UpdateSettings(DungeonManager.instance.difficultyMultipler);
            currentEnemies.Add(enemy);
            enemy.OnDeath += HandleEnemyDeath;
        }
    }
    private void HandleEnemyDeath()
    {
        List<Enemy> deadEnemies = currentEnemies.FindAll(e => e == null || e.isDead);
        foreach (Enemy dead in deadEnemies)
        {
            RemoveEnemy(dead);
        }
    }

    private void RemoveEnemy(Enemy enemy)
    {
        enemy.OnDeath -= HandleEnemyDeath;
        currentEnemies.Remove(enemy);
    }

    private void SpawnInteractables()
    {
        if (maxInteratableCount < 1 || interactableSpawns.Length < 1 || interactablePrefabs.Length < 1)
        {
            Debug.Log("[Room] Cant spawn Interactables. (interactableCount is 0, or spawnCount is 0, or prefabCount is 0)");
            return;
        }

        // Create and populate a Vector2 array with spawn positions
        Vector2[] spawnPositions;
        if (interactableSpawns != null && interactableSpawns.Length > 0)
        {
            spawnPositions = new Vector2[interactableSpawns.Length];
            for (int i = 0; i < interactableSpawns.Length; i++)
            {
                spawnPositions[i] = interactableSpawns[i].position;
            }
        }
        else
        {
            spawnPositions = new Vector2[] { transform.position };
        }

        // Spawn enemies
        for (int i = 0; i < maxInteratableCount; i++)
        {
            GameObject prefab = interactablePrefabs[Random.Range(0, interactablePrefabs.Length)];
            Vector2 spawnPos = spawnPositions[Random.Range(0, spawnPositions.Length)];
            InstantiateInteractable(prefab, spawnPos);
        }
    }

    private void InstantiateInteractable(GameObject prefab, Vector3 position)
    {
        Instantiate(prefab, position, Quaternion.identity, transform);
    }

    public Vector3 GetPlayerSpawnPosition()
    {
        if (playerSpawn != null) return playerSpawn.position;
        else return transform.position;
    }
}
