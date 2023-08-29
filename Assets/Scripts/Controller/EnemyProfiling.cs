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

    public float Health { get; private set; }

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
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        CheckDespawn();

        if (Health > enemyData.maximumHealth) Health = enemyData.maximumHealth;
        //if (team != enemyData.team) team = enemyData.team;
    }

    public void SetEnemyData(Enemy enemy)
    {
        enemyData = enemy;
        Health = enemy.maximumHealth;
        team = enemy.team;
    }

    private void CheckDespawn()
    {
        if (Health <= 0) Destroy(gameObject);

        float distanceToDespawn = transform.position.x - Camera.main.transform.position.x;
        if (distanceToDespawn < -despawnDistance)
        {
            Destroy(gameObject);
            if (homeworld != null)
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
        Health -= Damage;
    }

}

