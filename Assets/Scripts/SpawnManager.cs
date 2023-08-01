
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public List<Enemy> enemyTypes; // List of different enemy types to spawn
    public GameObject enemyPrefab; // Reference to the enemy prefab in the Unity Editor

    public Transform[] spawnPoints; // Array of spawn points for enemies

    public float spawnInterval = 3f; // Time interval between enemy spawns
    private float nextSpawnTime;
    public LayerMask prefabLayer;

    private void Start()
    {
        nextSpawnTime = Time.time + spawnInterval;
    }

    private void Update()
    {
        if (Time.time >= nextSpawnTime)
        {
            SpawnRandomEnemy();
            nextSpawnTime = Time.time + spawnInterval;
        }
    }

    private void SpawnRandomEnemy()
    {
        if (enemyTypes.Count == 0 || spawnPoints.Length == 0)
            return;

        int randomEnemyIndex = UnityEngine.Random.Range(0, enemyTypes.Count);
        int randomSpawnPointIndex = UnityEngine.Random.Range(0, spawnPoints.Length);

        Enemy enemyData = enemyTypes[randomEnemyIndex];

        // Get the camera's position and add an offset to move it to the right side
        Vector3 cameraPosition = Camera.main.transform.position;
        Vector3 spawnOffset = new Vector3(10f, 0f, 0f); // Adjust the offset to your desired distance from the camera

        // Calculate the spawn position by adding the offset to the camera position and using the spawn point's position
        Vector3 randomDeviation = new Vector3(UnityEngine.Random.Range(20f, 40f), UnityEngine.Random.Range(-10f, 10f), 0f); // Adjust the ranges for the desired deviation
        Vector3 spawnPosition = cameraPosition + spawnOffset + spawnPoints[randomSpawnPointIndex].position + randomDeviation;
        spawnPosition.z = 0f;

        GameObject newEnemy = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
        newEnemy.GetComponent<EnemyProfiling>().enemyData = enemyData;
        newEnemy.layer = prefabLayer;
    }

    
}

