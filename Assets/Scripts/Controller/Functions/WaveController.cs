using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditorInternal.Profiling.Memory.Experimental.FileFormat;
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
        public List<Battleship> enemyBattleships;
        public List<int> enemyBattleshipCounts;
        public List<Vector3> spawnPoints;
        public List<Vector3> capitalSpawnPoints;
        public float enemySpawnInterval;

        public List<Enemy> allyTypes;
        public List<int> allyCounts;
        public List<Character> allyCharacters;
        public List<Battleship> allyBattleships;
        public List<int> allyBattleshipCounts;
        public List<Vector3> supportPoints;
        public List<Vector3> capitalSupportPoints;
        public float supportSpawnInterval;

        private int enemyCountAll = 0;
        public int enemyCasualties { get; private set; }
        private bool initializedEnemyCount = true;
        public int EnemyCountAll { 
            get {
                if (initializedEnemyCount)
                {
                    int totalEnemyCount = 0;
                    foreach (int count in enemyCounts)
                    {
                        totalEnemyCount += count;
                    }
                    foreach (int count in enemyBattleshipCounts)
                    {
                        totalEnemyCount += count;
                    }
                    enemyCountAll = totalEnemyCount;
                    initializedEnemyCount = false;
                    UnityEngine.Debug.Log("Set total Enemy Count for this wave successfully: " + enemyCountAll);
                }
                return enemyCountAll;
            } 
            protected set { } 
        }
        private int allyCountAll = 0;
        public int allyCasualties { get; private set; }
        private bool initializedAllyCount = true;
        public int AllyCountAll {
            get
            {
                if (initializedAllyCount) {
                    int totalAllyCount = 0;
                    foreach (int count in allyCounts)
                    {
                        totalAllyCount += count;
                    }
                    foreach (int count in allyBattleshipCounts)
                    {
                        totalAllyCount += count;
                    }
                    allyCountAll = totalAllyCount;
                    initializedAllyCount = false;
                    UnityEngine.Debug.Log("Set total Ally Count for this wave successfully: " + allyCountAll);
                }
                return allyCountAll;
            }
            protected set { }
        }
        

        public bool isBossWave;
        public bool isThereCritical;
        public bool allowPassiveSpawns;
        public float waveInterval;
        public enum SpawnBehaviour { RANDOM, SATURATED, CONCURRENT }
        public SpawnBehaviour spawnBehaviour;
        public enum ReinforcementBehaviour { RANDOM, SATURATED, CONCURRENT }
        public ReinforcementBehaviour reinBehavior;

        public void ResynchronizeTotalCounts()
        {
            initializedAllyCount = true;
            initializedEnemyCount = true;
            UnityEngine.Debug.Log("Resynchronize Counts initialized");
        }
        public void SetAllyCasualty()
        {
            allyCasualties += 1;
        }

        public void SetEnemyCasualty()
        {
            enemyCasualties += 1;
        }
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
    private int currentEnemyCapitalIndex = 0;
    private int currentAllyIndex = 0;
    private int currentAllyCapitalIndex = 0;
    private float nextWaveCountdown = 0;
    public bool bossPresent { get; private set; }

    private void Start()
    {
        passiveSpawnManager = GetComponent<SpawnManager>();
        StartWave(currentWaveIndex);
        
    }

    private void Update()
    {
        if (Time.time >= nextWaveCountdown && !bossPresent)
        {
            StartWave(currentWaveIndex);
        }
    }

    private void StartWave(int waveIndex)
    {
        if (waveIndex < waves.Count)
        {
            
            Wave currentWave = waves[waveIndex];
            bool syncEnemy = SynchronizeEnemyCounts(currentWave);
            bool syncAlly = SynchronizeAllyCounts(currentWave);
            if (syncAlly || syncEnemy) currentWave.ResynchronizeTotalCounts();
            nextWaveCountdown = Time.time + currentWave.waveInterval;

            passiveSpawnManager.SetActive(currentWave.allowPassiveSpawns);
                
            if (currentWave.EnemyCountAll > 0) StartCoroutine(SpawnEnemiesCoroutine(currentWave, waveIndex));
            if (currentWave.AllyCountAll > 0) StartCoroutine(SpawnAlliesCoroutine(currentWave, waveIndex));

            if (currentWave.isBossWave)
            {
                if (bossEntity.Length > 0) SpawnBoss(bossEntity[bossIndex], null);
                BossInterfaceHud.SetActive(true);
                bossPresent = true;
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

    public bool SynchronizeEnemyCounts(Wave wave)
    {
        bool needToRecalibrate = false;
        int targetCount = wave.enemyTypes.Count;
        int battleshipCount = wave.enemyBattleships.Count;

        while (wave.enemyCounts.Count < targetCount)
        {
            wave.enemyCounts.Add(10);
            if (!needToRecalibrate) needToRecalibrate = true;
        }

        while (wave.enemyBattleshipCounts.Count < battleshipCount)
        {
            wave.enemyBattleshipCounts.Add(1);
            if (!needToRecalibrate) needToRecalibrate = true;
        }

        while (wave.enemyCounts.Count > targetCount)
        {
            int lastIndex = wave.enemyCounts.Count - 1;
            wave.enemyCounts.RemoveAt(lastIndex);
            if (!needToRecalibrate) needToRecalibrate = true;
        }

        while (wave.enemyBattleshipCounts.Count > battleshipCount)
        {
            int lastIndex = wave.enemyBattleshipCounts.Count - 1;
            wave.enemyBattleshipCounts.RemoveAt(lastIndex);
            if (!needToRecalibrate) needToRecalibrate = true;
        }
        return needToRecalibrate;
    }

    public bool SynchronizeAllyCounts(Wave wave)
    {
        bool needToRecalibrate = false;
        int targetCount = wave.allyTypes.Count;
        int battleshipCount = wave.allyBattleships.Count;

        while (wave.allyCounts.Count < targetCount)
        {
            wave.allyCounts.Add(10);
            if (!needToRecalibrate) needToRecalibrate = true;
        }

        while (wave.allyBattleshipCounts.Count < battleshipCount)
        {
            wave.allyBattleshipCounts.Add(1);
            if (!needToRecalibrate) needToRecalibrate = true;
        }

        while (wave.allyCounts.Count > targetCount)
        {
            int lastIndex = wave.allyCounts.Count - 1;
            wave.allyCounts.RemoveAt(lastIndex);
            if (!needToRecalibrate) needToRecalibrate = true;
        }
        while (wave.allyBattleshipCounts.Count > battleshipCount)
        {
            int lastIndex = wave.allyBattleshipCounts.Count - 1;
            wave.allyBattleshipCounts.RemoveAt(lastIndex);
            if (!needToRecalibrate) needToRecalibrate = true;
        }
        return needToRecalibrate;
    }

    private IEnumerator SpawnAlliesCoroutine(Wave currentWave, int currentWaveIndex)
    {
        int spawnPointIndex = 0;
        Vector3 offset = Vector3.zero;
        for (int i = 0; i < currentWave.AllyCountAll; i++)
        {
            switch (currentWave.reinBehavior)
            {
                case Wave.ReinforcementBehaviour.RANDOM:
                    if (currentWave.supportPoints.Count > 0)
                    {
                        spawnPointIndex = Random.Range(0, currentWave.supportPoints.Count);
                        offset.x = Random.Range(-25, 25);
                        offset.y = Random.Range(-25, 25);
                        HandleAllySpawns(currentWave, spawnPointIndex, currentWaveIndex, offset);
                    }
                    if (currentWave.capitalSupportPoints.Count > 0)
                    {
                        spawnPointIndex = Random.Range(0, currentWave.capitalSupportPoints.Count);
                        offset.x = Random.Range(-25, 25);
                        offset.y = Random.Range(-25, 25);
                        HandleAllyCapitalSpawns(currentWave, spawnPointIndex, currentWaveIndex, offset);
                    }
                    break;
                case Wave.ReinforcementBehaviour.CONCURRENT:
                    if (currentWave.supportPoints.Count > 0)
                    {
                        for (int j = 0; j < currentWave.supportPoints.Count - 1; j++)
                        {
                            HandleAllySpawns(currentWave, j, currentWaveIndex, offset);
                        }
                    }
                    if (currentWave.capitalSupportPoints.Count > 0)
                    {
                        for (int j = 0; j < currentWave.capitalSupportPoints.Count - 1; j++)
                        {
                            HandleAllyCapitalSpawns(currentWave, j, currentWaveIndex, offset);
                        }
                    }
                    break;
                case Wave.ReinforcementBehaviour.SATURATED:
                    int entityIndex = 0, capitalIndex = 0;
                    if (currentWave.supportPoints.Count > 0)
                    {
                        spawnPointIndex = (entityIndex + 1) % (currentWave.supportPoints.Count - 1);
                        HandleAllySpawns(currentWave, spawnPointIndex, currentWaveIndex, offset);
                    }
                    if (currentWave.capitalSupportPoints.Count > 0)
                    {
                        spawnPointIndex = (capitalIndex + 1) % (currentWave.capitalSupportPoints.Count - 1);
                        HandleAllyCapitalSpawns(currentWave, spawnPointIndex, currentWaveIndex, offset);
                    }
                    break;
            }
            // You can introduce a delay here if needed
            yield return new WaitForSeconds(currentWave.supportSpawnInterval);
            // e.g., yield return new WaitForSeconds(enemySpawnDelay);
        }
        yield break;
    }

    private void HandleAllySpawns(Wave currentWave, int spawnPointIndex, int currentWaveIndex, Vector3 offset)
    {
        EnemyProfiling allyEntity = SpawnEntity(currentWave.allyTypes[currentAllyIndex], currentWave.supportPoints.ToArray()[spawnPointIndex] + offset);
        allyEntity.WaveIndex = currentWaveIndex;
        if (allyEntity.team != Team.ALLIES) allyEntity.SetTeam(Team.ALLIES);
        currentWave.allyCounts[currentAllyIndex] -= 1;
        if (currentWave.allyCounts[currentAllyIndex] <= 0) currentAllyIndex = (currentAllyIndex + 1) % currentWave.allyTypes.Count;
    }
    private void HandleAllyCapitalSpawns(Wave currentWave, int spawnPointIndex, int currentWaveIndex, Vector3 offset)
    {
        BattleshipProfiling battleshipEntity = SpawnBattleship(currentWave.allyBattleships[currentAllyCapitalIndex], currentWave.capitalSupportPoints.ToArray()[spawnPointIndex] + offset);
        battleshipEntity.WaveIndex = currentWaveIndex;
        if (battleshipEntity.team != Team.ALLIES) battleshipEntity.SetTeam(Team.ALLIES);
        currentWave.allyBattleshipCounts[currentAllyCapitalIndex] -= 1;
        if (currentWave.allyBattleshipCounts[currentAllyCapitalIndex] <= 0) currentAllyCapitalIndex = (currentAllyCapitalIndex + 1) % currentWave.allyBattleships.Count;
    }
    private IEnumerator SpawnEnemiesCoroutine(Wave currentWave, int currentWaveIndex)
    {
        UnityEngine.Debug.Log("Wave "+ currentWaveIndex + " enemy spawn coroutine is active");
        int spawnPointIndex = 0;
        Vector3 offset = Vector3.zero;
        for (int i = 0; i < currentWave.EnemyCountAll; i++)
        {
            switch (currentWave.spawnBehaviour)
            {
                case Wave.SpawnBehaviour.RANDOM:
                    if (currentWave.spawnPoints.Count > 0)
                    {
                        spawnPointIndex = Random.Range(0, currentWave.spawnPoints.Count);
                        offset.x = Random.Range(-25, 25);
                        offset.y = Random.Range(-25, 25);
                        HandleEnemySpawns(currentWave, spawnPointIndex, currentWaveIndex, offset);
                    }
                    if (currentWave.capitalSpawnPoints.Count > 0)
                    {
                        spawnPointIndex = Random.Range(0, currentWave.capitalSpawnPoints.Count);
                        offset.x = Random.Range(-25, 25);
                        offset.y = Random.Range(-25, 25);
                        HandleEnemyCapitalSpawns(currentWave, spawnPointIndex, currentWaveIndex, offset);
                    }
                    break;
                case Wave.SpawnBehaviour.CONCURRENT:
                    if(currentWave.spawnPoints.Count > 0)
                    for (int j = 0; j < currentWave.spawnPoints.Count - 1; j++)
                    {
                        HandleEnemySpawns(currentWave, j, currentWaveIndex, offset);
                    }
                    if(currentWave.capitalSpawnPoints.Count > 0)
                    for (int j = 0; j < currentWave.capitalSpawnPoints.Count - 1; j++)
                    {
                        HandleEnemyCapitalSpawns(currentWave, j, currentWaveIndex, offset);
                    }
                    break;
                case Wave.SpawnBehaviour.SATURATED:
                    int entityIndex = 0, capitalIndex = 0;

                    if (currentWave.spawnPoints.Count > 0)
                    {
                        spawnPointIndex = (entityIndex + 1) % (currentWave.spawnPoints.Count - 1);
                        HandleEnemySpawns(currentWave, spawnPointIndex, currentWaveIndex, offset);
                    }

                    if (currentWave.capitalSpawnPoints.Count > 0)
                    {
                        spawnPointIndex = (capitalIndex + 1) % (currentWave.capitalSpawnPoints.Count - 1);
                        HandleEnemyCapitalSpawns(currentWave, spawnPointIndex, currentWaveIndex, offset);
                    }
                    break;
            }
            yield return new WaitForSeconds(currentWave.enemySpawnInterval);
        }
        yield break;
    }

    private void HandleEnemySpawns(Wave currentWave, int spawnPointIndex, int currentWaveIndex, Vector3 offset)
    {
        EnemyProfiling enemyEntity = SpawnEntity(currentWave.enemyTypes[currentEnemyIndex], currentWave.spawnPoints.ToArray()[spawnPointIndex] + offset);
        enemyEntity.WaveIndex = currentWaveIndex;
        currentWave.enemyCounts[currentEnemyIndex] -= 1;
        if (currentWave.enemyCounts[currentEnemyIndex] <= 0) currentEnemyIndex = (currentEnemyIndex + 1) % currentWave.enemyTypes.Count;
    }

    private void HandleEnemyCapitalSpawns(Wave currentWave, int spawnPointIndex, int currentWaveIndex, Vector3 offset)
    {
        BattleshipProfiling battleshipEntity = SpawnBattleship(currentWave.enemyBattleships[currentEnemyCapitalIndex], currentWave.capitalSpawnPoints.ToArray()[spawnPointIndex] + offset);
        battleshipEntity.WaveIndex = currentWaveIndex;
        currentWave.enemyBattleshipCounts[currentEnemyCapitalIndex] -= 1;
        if (currentWave.enemyBattleshipCounts[currentEnemyCapitalIndex] <= 0) currentEnemyCapitalIndex = (currentEnemyCapitalIndex + 1) % currentWave.enemyBattleships.Count;
    }
    private EnemyProfiling SpawnEntity(Enemy enemyType, Vector3 spawnPoint)
    {
        GameObject newEnemy = Instantiate(enemyType.enemyPrefab, spawnPoint, Quaternion.identity);
        newEnemy.GetComponent<EnemyProfiling>().SetEnemyData(enemyType);
        newEnemy.layer = prefabLayer;
        newEnemy.tag = "MainWavesForce";

        return newEnemy.GetComponent<EnemyProfiling>();
    }

    private BattleshipProfiling SpawnBattleship(Battleship battleship, Vector3 spawnPoint)
    {
        GameObject newBattleship = Instantiate(battleship.batteshipPrefab, spawnPoint, Quaternion.identity);
        newBattleship.GetComponent<BattleshipProfiling>().SetBattleshipProperty(battleship);
        newBattleship.layer = prefabLayer;
        newBattleship.tag = "MainWavesForce";

        return newBattleship.GetComponent<BattleshipProfiling>();
    }

    private void SpawnBoss(Object boss, Vector3? spawnCoord)
    {
        Vector3 camera = Camera.main.transform.position + new Vector3(100f, 0f, 10f);
        if (boss is Enemy entityBoss)
        {
            HandleBossSpawn(entityBoss, entityBoss.enemyPrefab, spawnCoord == null ? camera : (Vector3) spawnCoord);
        } 
        else if (boss is Character characterBoss)
        {
            HandleBossSpawn(characterBoss, characterBoss.characterPrefab, spawnCoord == null ? camera : (Vector3)spawnCoord);
        } 
        else if (boss is Battleship battleshipBoss)
        {
            HandleBossSpawn(battleshipBoss, battleshipBoss.batteshipPrefab, spawnCoord == null ? camera : (Vector3)spawnCoord);
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
        BattleshipProfiling battleshipProfiling = boss.GetComponent<BattleshipProfiling>();

        if (enemyProfiling != null)
        {
            enemyProfiling.SetEnemyData((Enemy)obj);
            BossInterfaceHud.GetComponent<BossBarFunction>().SetBossProfile(enemyProfiling);
        }
        if (characterProfiling != null)
        {
            characterProfiling.GetCharacterFromScriptableObject((Character)obj);
            BossInterfaceHud.GetComponent<BossBarFunction>().SetBossProfile(characterProfiling);
        }
        if (battleshipProfiling != null)
        {
            battleshipProfiling.SetBattleshipProperty((Battleship)obj);
            BossInterfaceHud.GetComponent<BossBarFunction>().SetBossProfile(battleshipProfiling);
        }
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

    public void RecordAllyCasualties(int WaveIndex)
    {
        waves[WaveIndex].SetAllyCasualty();
    }

    public void RecordEnemyCasualties(int WaveIndex)
    {
        waves[WaveIndex].SetEnemyCasualty();
    }

    public void InformBossDead()
    {
        StartWave(currentWaveIndex);
        bossPresent = false;
    }
}
