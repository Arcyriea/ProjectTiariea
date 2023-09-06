using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using System.Diagnostics;
using UnityEngine;

public class EnemyProfiling : MonoBehaviour
{
    public Enemy enemyData { get; private set; }
    private Animator animator;
    protected AudioSource audioSource;
    private SpriteRenderer spriteRenderer;
    private float despawnDistance = 150f;
    private HomeworldHearts homeworld;
    public Enums.Team team { get; private set; } // This variable can be temporarly set to a different team in case of dealing with entities that are capable to convert or mind control to their side

    //new arguments
    protected float meleeDetectionRange = 0.2f; // Adjust the detection range as needed
    protected bool inMeleeRange = false;
    protected float lastAttackTime = 0f;
    public bool punctured = false;
    private int punctureCooldown;
    //
    public int? WaveIndex = null;
    public float Health { get; private set; }
    public float Shield { get; private set; }
    private class StatusEffect
    {
        public Enums.StatusEffectType type;
        public int stackCount;
        public float duration;

        public StatusEffect(Enums.StatusEffectType type, int stackCount, float duration)
        {
            this.type = type;
            this.stackCount = stackCount;
            this.duration = duration;
        }
    }

    private LinkedList<StatusEffect> statusEffects = new LinkedList<StatusEffect>();

    // Start is called before the first frame update
    protected virtual void Start()
    {
        homeworld = GameObject.Find("Main Camera")?.GetComponent<HomeworldHearts>();
        animator = enemyData == null ? null : gameObject.GetComponent<Animator>();
        audioSource = gameObject.GetComponent<AudioSource>();
        if (team == Enums.Team.ALLIES) transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z); 
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        CheckDespawn();

        if (punctured && punctureCooldown <= 0)
        {
            punctureCooldown = 3;
        }

        punctureCooldown -= (int)Time.fixedDeltaTime;
        if (punctureCooldown <= 0) punctured = false;

        if (Health > enemyData.maximumHealth) Health = enemyData.maximumHealth;
        if (Shield > enemyData.maximumShield) Shield = enemyData.maximumShield;
    }

    public void SetEnemyData(Enemy enemy)
    {
        enemyData = enemy;
        Health = enemy.maximumHealth;
        Shield = enemy.maximumShield;
        team = enemy.team;
    }

    private void CheckDespawn()
    {
        if (Health <= 0) Destroy(gameObject);

        float distanceToDespawn = transform.position.x - Camera.main.transform.position.x;
        if (team == Enums.Team.ALLIES) distanceToDespawn = -distanceToDespawn;
        if (distanceToDespawn < -despawnDistance)
        {
            Destroy(gameObject);
            if (homeworld != null && team != Enums.Team.ALLIES)
            {
                homeworld.heart -= 1;
            }
            else
            {
                UnityEngine.Debug.LogWarning("HomeworldHearts component not found on Main Camera.");
            }
        }
    }

    public virtual void EnemyAction(string action)
    {
        UnityEngine.Debug.Log("Got into EnemyAction");
    }

    public void SetTeam(Enums.Team team)
    {
        this.team = team;
    }

    public void TakeDamage(float Damage)
    {
        if (Shield > 0) Shield -= Damage;
        
        else Health -= Shield < 0 ? (Shield + Damage) : Damage;
    }

    protected virtual void OnDestroy()
    {
        WaveController waveController = null;
        if ((tag == "Boss" || tag == "MainWavesForce") && FindFirstObjectByType<WaveController>() != null) 
            waveController = FindFirstObjectByType<WaveController>();

            if (team != Enums.Team.ALLIES) {
                if (FindFirstObjectByType<PartyController>() != null)
                {
                    PartyController controller = FindFirstObjectByType<PartyController>();
                    foreach (GameObject character in controller.spawnedPrefabs)
                    {
                        if (character != null)
                        {
                            CharacterProfiling profiling = character.GetComponent<CharacterProfiling>();
                            if (profiling != null) {
                                if (profiling.isDead) continue;
                                profiling.IncreaseUltMeter(10f);
                                UnityEngine.Debug.Log("Increased Ultimate Meter for " + profiling.character.characterName);
                            }
                            
                        }
                    }
                }
                

                if (waveController != null && tag == "MainWavesForce")
                {
                    if (WaveIndex != null) waveController.RecordEnemyCasualties((int) WaveIndex);
                }

                if (waveController != null && tag == "Boss") 
                {
                    waveController.BossInterfaceHud.GetComponent<BossBarFunction>().RemoveBoss();
                    waveController.InformBossDead();
                    UnityEngine.Debug.Log("Boss Declared Dead");
                }
            }
            else if (team == Enums.Team.ALLIES)
            {
                if (waveController != null && tag == "MainWavesForce")
                {
                    if (WaveIndex != null) waveController.RecordAllyCasualties((int)WaveIndex);
                }
            }
    }
}

