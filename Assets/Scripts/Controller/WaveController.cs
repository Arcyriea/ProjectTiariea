using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Enums;

public class WaveController : MonoBehaviour
{
    [System.Serializable]
    public class Wave
    {
        public List<Enemy> enemyTypes;
        public List<Character> enemyCharacters;
        public List<Vector3> spawnPoints;

        public List<Enemy> allyTypes;
        public List<Character> allyCharacters;
        public List<Vector3> supportPoints;
        public int enemyCount;
        public int allyCount;
        public bool isBossWave;
        public bool allowPassiveSpawns;
        public enum SpawnBehaviour { RANDOM, SATURATED, CONCURRENT }
        public SpawnBehaviour spawnBehaviour;
        public enum ReinforcementBehaviour { RANDOM, SATURATED, CONCURRENT }
        public ReinforcementBehaviour reinBehavior;
    }

    private SpawnManager passiveSpawnManager;
    public List<Wave> waves;
    public Character bossCharacter;
    public Character criticalCharacter;

    public LayerMask prefabLayer;

    private int currentWaveIndex = 0;
    private int currentEnemyIndex = 0;

    private void Start()
    {
        passiveSpawnManager = GetComponent<SpawnManager>();
        StartWave(currentWaveIndex);
    }

    private void StartWave(int waveIndex)
    {
        if (waveIndex < waves.Count)
        {
            
            Wave currentWave = waves[waveIndex];

            
                passiveSpawnManager.SetActive(currentWave.allowPassiveSpawns);
            int spawnPointIndex = 0;
            
                
            for (int i = 0; i < currentWave.enemyCount; i++)
            {
                switch (currentWave.spawnBehaviour)
                {
                    case Wave.SpawnBehaviour.RANDOM:
                        spawnPointIndex = Random.Range(0, currentWave.spawnPoints.Count);
                        break;
                    case Wave.SpawnBehaviour.CONCURRENT:

                        break;
                    case Wave.SpawnBehaviour.SATURATED:
                        spawnPointIndex = (spawnPointIndex + 1) % (currentWave.spawnPoints.Count - 1);
                        break;
                }
                SpawnEnemy(currentWave.enemyTypes[currentEnemyIndex], currentWave.spawnPoints.ToArray()[spawnPointIndex]);
                currentEnemyIndex = (currentEnemyIndex + 1) % currentWave.enemyTypes.Count;
            }

            if (currentWave.isBossWave)
            {
                SpawnBoss();
            }
        }
        else
        {
            // All waves completed, game over or victory condition
        }
    }

    private void SpawnEnemy(Enemy enemyType, Vector3 spawnPoint)
    {
        // Logic to spawn an enemy of the given type
        GameObject newEnemy = Instantiate(enemyType.enemyPrefab, spawnPoint, Quaternion.identity);
        newEnemy.GetComponent<EnemyProfiling>().SetEnemyData(enemyType);
        newEnemy.layer = prefabLayer;
    }

    private void SpawnBoss()
    {
        // Logic to spawn a boss character
    }

    // Other methods for wave completion, handling enemies, etc.
}
