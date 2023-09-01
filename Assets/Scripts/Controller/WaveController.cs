using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
        public float enemySpawnInterval;

        public List<Enemy> allyTypes;
        public List<Character> allyCharacters;
        public List<Vector3> supportPoints;
        public float supportSpawnInterval;

        public int enemyCount;
        public int allyCount;
        public bool isBossWave;
        public bool isThereCritical;
        public bool allowPassiveSpawns;
        public float waveInterval;
        public enum SpawnBehaviour { RANDOM, SATURATED, CONCURRENT }
        public SpawnBehaviour spawnBehaviour;
        public enum ReinforcementBehaviour { RANDOM, SATURATED, CONCURRENT }
        public ReinforcementBehaviour reinBehavior;
    }

    private SpawnManager passiveSpawnManager;
    public List<Wave> waves;
    public Object[] bossEntity;
    public Object[] criticalEntity;

    public LayerMask prefabLayer;

    private int bossIndex = 0;
    private int critIndex = 0;
    private int currentWaveIndex = 0;
    private int currentEnemyIndex = 0;
    private float spawnDelayCountdown = 0;
    private float supportDelayCountdown = 0;
    private float nextWaveCountdown = 0;
    private void Start()
    {
        passiveSpawnManager = GetComponent<SpawnManager>();
        StartWave(currentWaveIndex);
        
    }

    private void Update()
    {
        if (Time.time >= nextWaveCountdown)
        {
            StartWave(currentWaveIndex);
        }
    }

    private void StartWave(int waveIndex)
    {
        if (waveIndex < waves.Count)
        {
            
            Wave currentWave = waves[waveIndex];
            spawnDelayCountdown = Time.time + currentWave.enemySpawnInterval;
            supportDelayCountdown = Time.time + currentWave.supportSpawnInterval;
            nextWaveCountdown = Time.time + currentWave.waveInterval;

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
                SpawnBoss(null, null);
            }

            currentWaveIndex += 1;
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

    private void SpawnBoss(Object boss, Vector3? spawnCoord)
    {
        if (boss is Enemy entityBoss)
        {
            HandleBossSpawn(entityBoss, entityBoss.enemyPrefab, spawnCoord == null ? new Vector3() : (Vector3) spawnCoord);
        } 
        else if (boss is Character characterBoss)
        {
            HandleBossSpawn(characterBoss, characterBoss.characterPrefab, spawnCoord == null ? new Vector3() : (Vector3)spawnCoord);
        } 
        else
        {
            throw new InvalidImplementationException();
        }
    }

    private void HandleBossSpawn(Object obj, GameObject prefab, Vector3 spawnCoord)
    {
        GameObject boss = Instantiate(prefab, spawnCoord, Quaternion.identity);
        EnemyProfiling enemyProfiling = boss.GetComponent<EnemyProfiling>();
        CharacterProfiling characterProfiling = boss.GetComponent<CharacterProfiling>();

        if (enemyProfiling != null) enemyProfiling.SetEnemyData((Enemy)obj);
        if (characterProfiling != null) characterProfiling.GetCharacterFromScriptableObject((Character)obj);

        boss.layer = prefabLayer;
    }
    private void SpawnImportant()
    {
        // Logic to spawn important entity to protect
    }

    private void HandleImportantSpawn(Object obj, GameObject prefab, Vector3 spawnCoord)
    {

    }
    // Other methods for wave completion, handling enemies, etc.
}
