
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

        int randomEnemyIndex = Random.Range(0, enemyTypes.Count);
        int randomSpawnPointIndex = Random.Range(0, spawnPoints.Length);

        Enemy enemyData = enemyTypes[randomEnemyIndex];

        // Get the camera's position and add an offset to move it to the right side
        Vector3 cameraPosition = Camera.main.transform.position;
        Vector3 spawnOffset = new Vector3(10f, 0f, 0f); // Adjust the offset to your desired distance from the camera

        // Calculate the spawn position by adding the offset to the camera position and using the spawn point's position
        Vector3 spawnPosition = cameraPosition + spawnOffset + spawnPoints[randomSpawnPointIndex].position;

        GameObject newEnemy = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
        newEnemy.GetComponent<EnemyProfiling>().enemyData = enemyData;
    }
}

