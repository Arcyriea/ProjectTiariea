using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Enums;

public class BattleshipProfiling : MonoBehaviour, EffectsManager
{
    private SpriteRenderer spriteRenderer;
    protected Animator animator;
    protected AudioSource audioSource;
    private HomeworldHearts homeworld;
    public Team team { get; private set; }

    protected LinkedList<EffectsManager.StatusEffect> statusEffects = new LinkedList<EffectsManager.StatusEffect>();
    protected List<GameObject> calibratingSubsystems = new List<GameObject>();
    public Battleship battleshipProperty { get; private set; }
    protected float lastAttackTime = 0f;

    public int? WaveIndex = null;
    bool exploding = false;
    public GameObject barPrefab { get; set; }
    public HealthBar healthBar { get; protected set; }

    public float Health { get; private set; }
    public float Shield { get; private set; }

    private float despawnDistance = 150f;

    protected GameObject mainTarget = null;
    public GameObject MainTarget { get { return mainTarget; } protected set { mainTarget = MainTarget; } }
    // Start is called before the first frame update
    protected virtual void Start()
    {
        homeworld = GameObject.Find("Main Camera")?.GetComponent<HomeworldHearts>();
        animator = battleshipProperty == null ? null : gameObject.GetComponent<Animator>();
        audioSource = gameObject.GetComponent<AudioSource>();

        if (team != Enums.Team.ALLIES) transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
        InitializeSubsystems();
    }
    protected bool trackingTarget = false;
    // Update is called once per frame
    protected virtual void Update()
    {
        CheckDespawn();
        if (Health > battleshipProperty.maximumHealth) Health = battleshipProperty.maximumHealth;
        if (Shield > battleshipProperty.maximumShield) Shield = battleshipProperty.maximumShield;
    }

    public void SetTeam(Enums.Team team)
    {
        this.team = team;
    }

    public void SetBattleshipProperty(Battleship battleship)
    {
        battleshipProperty = battleship;
        Health = battleship.maximumHealth;
        Shield = battleship.maximumShield;
        team = battleship.team;
    }
    protected bool CanAttack()
    {
        return Time.time - lastAttackTime >= battleshipProperty.attackCooldown;
    }
    private void CheckDespawn()
    {
        if (Health <= 0 && !exploding)
        {
            exploding = true;
            UnityEngine.Debug.Log("Battleship is falling");
            Destroy(gameObject, 4f);
        }

        float distanceToDespawn = transform.position.x - Camera.main.transform.position.x;
        if (team == Enums.Team.ALLIES) distanceToDespawn = -distanceToDespawn;
        if (distanceToDespawn < -despawnDistance)
        {
            Destroy(gameObject);
            if (homeworld != null && team != Team.ALLIES)
            {
                homeworld.heart -= 5;
            }
            else
            {
                UnityEngine.Debug.LogWarning("HomeworldHearts component not found on Main Camera.");
            }
        }
    }

    public void TakeDamage(float Damage)
    {
        if (Shield > 0) Shield -= Damage;

        else Health -= Shield < 0 ? (Shield + Damage) : Damage;
    }

    protected virtual void InitializeSubsystems()
    {
        foreach (GameObject subsystem in calibratingSubsystems)
        {
            if (subsystem != null)
            {
                SubsystemProfiling module = subsystem.GetComponent<SubsystemProfiling>();
                if (module == null) continue;

                module.SetTeam(team);
                module.SetParent(gameObject);
            }
        }
    }

    protected void FindTargets()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, battleshipProperty.attackRange);

        foreach (Collider2D collider in colliders)
        {
            if (collider != null)
            {
                BattleshipProfiling battleshipProfiling = collider.gameObject.GetComponent<BattleshipProfiling>();
                CharacterProfiling characterProfiling = collider.gameObject.GetComponent<CharacterProfiling>();

                if (battleshipProfiling != null && battleshipProfiling.team != team)
                {
                    if (mainTarget == null)
                    {
                        mainTarget = collider.gameObject;
                        SetMainWeaponTargets(mainTarget);
                    }
                }
                if (characterProfiling != null && characterProfiling.team != team && !characterProfiling.isDead)
                {
                    if (mainTarget == null)
                    {
                        mainTarget = collider.gameObject;
                        SetMainWeaponTargets(mainTarget);
                    }
                }
            }
        }

    }

    protected virtual void SetMainWeaponTargets(GameObject target)
    {
        throw new System.NotImplementedException();
    }

    protected IEnumerator TrackingMainTarget()
    {
        while (true)
        {
            if (mainTarget != null)
            {
                float distance = Vector3.Distance(transform.position, mainTarget.transform.position);

                CharacterProfiling characterProfiling = mainTarget.GetComponent<CharacterProfiling>();
                BattleshipProfiling battleshipProfiling = mainTarget.GetComponent <BattleshipProfiling>();

                if (characterProfiling != null)
                {
                    if (characterProfiling.isDead)
                    {
                        mainTarget = null; // Set mainTarget to null if out of range
                        SetMainWeaponTargets(mainTarget);
                        trackingTarget = false;
                        yield break;
                    }
                }

                if (battleshipProfiling != null)
                {
                    if (battleshipProfiling.exploding)
                    {
                        mainTarget = null; // Set mainTarget to null if out of range
                        SetMainWeaponTargets(mainTarget);
                        trackingTarget = false;
                        yield break;
                    }
                }
                // Check if the mainTarget is out of range (you can replace the threshold distance)
                if (distance > battleshipProperty.attackRange)
                {
                    mainTarget = null; // Set mainTarget to null if out of range
                    SetMainWeaponTargets(mainTarget);
                    trackingTarget = false;
                    yield break;
                }
                yield return null;
            }
            else yield break;
        }
    }

    protected virtual void FireAtMainTarget()
    {
        throw new System.NotImplementedException();
    }

    protected void AttackMode(string attackType)
    {
        Collider2D[] hitColliders;


        hitColliders = Physics2D.OverlapCircleAll(transform.position, battleshipProperty.attackRange);



        //For Single Attack
        switch (attackType)
        {
            case "melee":
                PerformAttack();
                break;
            case "ranged":
                PerformRanged();
                break;
        }

        lastAttackTime = Time.time;


        //For attack all
        foreach (Collider2D hitCollider in hitColliders)
        {
            if (IsEnemy(hitCollider.gameObject))
            {

            }
        }
    }

    protected bool IsEnemy(GameObject other)
    {
        BattleshipProfiling battleship = other.GetComponent<BattleshipProfiling>();
        CharacterProfiling character = other.GetComponent<CharacterProfiling>();
        EnemyProfiling entity = other.GetComponent<EnemyProfiling>();
        SubsystemProfiling subsystem = other.GetComponent<SubsystemProfiling>();

        if (battleship != null) { if (battleship.team != team) return true; }
        if (character != null) { if (character.team != team) return true; }
        if (entity != null) { if (entity.team != team) return true; }
        if (subsystem != null) { if (subsystem.team != team) return true; }

        return false;
    }



    protected virtual void PerformAttack()
    {
        throw new System.NotImplementedException();
    }

    protected virtual void PerformRanged()
    {
        throw new System.NotImplementedException();
    }

    protected virtual void PerformHeal()
    {
        throw new System.NotImplementedException();
    }

    protected virtual void PerformUltimate()
    {
        throw new System.NotImplementedException();
    }
    protected virtual void OnDestroy()
    {
        WaveController waveController = null;
        if ((tag == "Boss" || tag == "MainWavesForce") && FindFirstObjectByType<WaveController>() != null)
            waveController = FindFirstObjectByType<WaveController>();

        if (team != Enums.Team.ALLIES)
        {
            if (FindFirstObjectByType<PartyController>() != null)
            {
                PartyController controller = FindFirstObjectByType<PartyController>();
                foreach (GameObject character in controller.spawnedPrefabs)
                {
                    if (character != null)
                    {
                        CharacterProfiling profiling = character.GetComponent<CharacterProfiling>();
                        if (profiling != null)
                        {
                            if (profiling.isDead) continue;
                            profiling.IncreaseUltMeter(10f);
                            UnityEngine.Debug.Log("Increased Ultimate Meter for " + profiling.character.characterName);
                        }

                    }
                }
            }

            if (waveController != null && tag == "MainWavesForce")
            {
                if (WaveIndex != null) waveController.RecordEnemyCasualties((int)WaveIndex);
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

        foreach (GameObject subsystem in calibratingSubsystems)
        {
            if (subsystem != null)
                Destroy(subsystem);
        }
    }
}
