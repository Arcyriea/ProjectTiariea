using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using static Enums;

public class WaveController : MonoBehaviour
{
    [System.Serializable]
    public class Wave
    {
        public List<Enemy> enemyTypes;
        public List<int> enemyCounts;
        public List<Character> enemyCharacters;
        public List<Vector3> spawnPoints;
        public float enemySpawnInterval;

        public List<Enemy> allyTypes;
        public List<int> allyCounts;
        public List<Character> allyCharacters;
        public List<Vector3> supportPoints;
        public float supportSpawnInterval;

        public int enemyCountAll { 
            get {
                int totalEnemyCount = 0;
                foreach (int count in enemyCounts)
                {
                    totalEnemyCount += count;
                }
                return totalEnemyCount;
            } 
            private set { } 
        }
        public int allyCountAll {
            get
            {
                int totalAllyCount = 0;
                foreach (int count in allyCounts)
                {
                    totalAllyCount += count;
                }
                return totalAllyCount;
            }
            private set { }
        }
        public bool isBossWave;
        public bool isThereCritical;
        public bool allowPassiveSpawns;
        public float waveInterval;
        public enum SpawnBehaviour { RANDOM, SATURATED, CONCURRENT }
        public SpawnBehaviour spawnBehaviour;
        public enum ReinforcementBehaviour { RANDOM, SATURATED, CONCURRENT }
        public ReinforcementBehaviour reinBehavior;
    }

    public GameObject BossInterfaceHud;
    private SpawnManager passiveSpawnManager;
    public List<Wave> waves;
    public Object[] bossEntity;
    public Object[] criticalEntity;

    public LayerMask prefabLayer;

    private int bossIndex = 0;
    private int critIndex = 0;
    private int currentWaveIndex = 0;
    private int currentEnemyIndex = 0;
    private int currentAllyIndex = 0;
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
            nextWaveCountdown = Time.time + currentWave.waveInterval;

            passiveSpawnManager.SetActive(currentWave.allowPassiveSpawns);
                
            if (currentWave.enemyCountAll > 0) StartCoroutine(SpawnEnemiesCoroutine(currentWave));
            if (currentWave.allyCountAll > 0) StartCoroutine(SpawnAlliesCoroutine(currentWave));

            if (currentWave.isBossWave)
            {
                if (bossEntity.Length > 0) SpawnBoss(bossEntity[bossIndex], null);
                BossInterfaceHud.SetActive(true);
            }

            if (currentWave.isThereCritical)
            {
                if (criticalEntity.Length > 0) SpawnImportant(criticalEntity[critIndex], null);
            }

            currentWaveIndex += 1;
        }
        else
        {
            // All waves completed, game over or victory condition
        }
    }

    private IEnumerator SpawnAlliesCoroutine(Wave currentWave)
    {
        int spawnPointIndex = 0;

        for (int i = 0; i < currentWave.allyCountAll; i++)
        {
            switch (currentWave.reinBehavior)
            {
                case Wave.ReinforcementBehaviour.RANDOM:
                    spawnPointIndex = Random.Range(0, currentWave.supportPoints.Count);
                    break;
                case Wave.ReinforcementBehaviour.CONCURRENT:

                    break;
                case Wave.ReinforcementBehaviour.SATURATED:
                    spawnPointIndex = (spawnPointIndex + 1) % (currentWave.supportPoints.Count - 1);
                    break;
            }

            SpawnEnemy(currentWave.allyTypes[currentAllyIndex], currentWave.supportPoints.ToArray()[spawnPointIndex]);
            currentWave.allyCounts[currentAllyIndex] -= 1;
            if (currentWave.allyCounts[currentAllyIndex] <= 0) currentAllyIndex = (currentAllyIndex + 1) % currentWave.allyTypes.Count;

            // You can introduce a delay here if needed
            yield return new WaitForSeconds(currentWave.supportSpawnInterval);
            // e.g., yield return new WaitForSeconds(enemySpawnDelay);
        }
    }


    private IEnumerator SpawnEnemiesCoroutine(Wave currentWave)
    {
        int spawnPointIndex = 0;

        for (int i = 0; i < currentWave.enemyCountAll; i++)
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
            currentWave.enemyCounts[currentEnemyIndex] -= 1;
            if (currentWave.enemyCounts[currentEnemyIndex] <= 0) currentEnemyIndex = (currentEnemyIndex + 1) % currentWave.enemyTypes.Count;

            // You can introduce a delay here if needed
            yield return new WaitForSeconds(currentWave.enemySpawnInterval);
            // e.g., yield return new WaitForSeconds(enemySpawnDelay);
        }
    }
    private void SpawnEnemy(Enemy enemyType, Vector3 spawnPoint)
    {
        // Logic to spawn an enemy of the given type
        GameObject newEnemy = Instantiate(enemyType.enemyPrefab, spawnPoint, Quaternion.identity);
        newEnemy.GetComponent<EnemyProfiling>().SetEnemyData(enemyType);
        newEnemy.layer = prefabLayer;
        newEnemy.tag = "MainWavesForce";
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
            if (boss == null) UnityEngine.Debug.LogError("No data defined to instantiate a boss");
            UnityEngine.Debug.LogError("Boss data type assignment may be invalid");
            throw new InvalidImplementationException();
            
        }
        bossIndex++;
    }

    private void HandleBossSpawn(Object obj, GameObject prefab, Vector3 spawnCoord)
    {
        GameObject boss = Instantiate(prefab, spawnCoord, Quaternion.identity);
        EnemyProfiling enemyProfiling = boss.GetComponent<EnemyProfiling>();
        CharacterProfiling characterProfiling = boss.GetComponent<CharacterProfiling>();

        if (enemyProfiling != null) enemyProfiling.SetEnemyData((Enemy)obj);
        if (characterProfiling != null) characterProfiling.GetCharacterFromScriptableObject((Character)obj);

        boss.layer = prefabLayer;
        boss.tag = "Boss";
    }
    private void SpawnImportant(Object critical, Vector3? spawnCoord)
    {
        // Logic to spawn important entity to protect
        if (critical is Enemy entityCrit)
        {
            HandleImportantSpawn(entityCrit, entityCrit.enemyPrefab, spawnCoord == null ? new Vector3() : (Vector3)spawnCoord);
        }
        else if (critical is Character characterCrit)
        {
            HandleImportantSpawn(characterCrit, characterCrit.characterPrefab, spawnCoord == null ? new Vector3() : (Vector3)spawnCoord);
        }
        else
        {
            if (critical == null) UnityEngine.Debug.LogError("No data defined to instantiate the critical entity");
            UnityEngine.Debug.LogError("Critical entity data type assignment may be invalid");
            throw new InvalidImplementationException();
        }
        critIndex++;
    }

    private void HandleImportantSpawn(Object obj, GameObject prefab, Vector3 spawnCoord)
    {
        GameObject boss = Instantiate(prefab, spawnCoord, Quaternion.identity);
        EnemyProfiling enemyProfiling = boss.GetComponent<EnemyProfiling>();
        CharacterProfiling characterProfiling = boss.GetComponent<CharacterProfiling>();

        if (enemyProfiling != null)
        {
            enemyProfiling.SetEnemyData((Enemy)obj);
            if (enemyProfiling.team != Team.ALLIES) enemyProfiling.SetTeam(Team.ALLIES);
        }
        if (characterProfiling != null)
        {
            characterProfiling.GetCharacterFromScriptableObject((Character)obj);
            if (characterProfiling.team != Team.ALLIES) characterProfiling.SetTeam(Team.ALLIES);
        }

        boss.layer = prefabLayer;
        boss.tag = "Important";
    }
    // Other methods for wave completion, handling enemies, etc.

    public void RecordAllyCasualties()
    {
        
    }
}
