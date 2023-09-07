using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;


public class NiexpieriaBoss : EnemyProfiling
{
    public MissileProperties missile;
    public AudioClip missileLaunchClip;

    public GameObject mainTurret;
    public GameObject[] pulseTurrets;
    public GameObject[] beamfarerTurrets;
    public Transform[] launchPorts;
    public Transform[] torpedoPorts;

    private GameObject mainTarget = null;
    public GameObject MainTarget { get { return mainTarget; } private set { mainTarget = MainTarget; } }


    private List<GameObject> calibratingSubsystems = new List<GameObject>();
    public override void EnemyAction(string action)
    {
        switch (action)
        {
            case "attack":
                PerformAttack();
                break;
            case "heal":
                PerformHeal();
                break;
            case "ranged":
                PerformRanged();
                break;
            case "ultimate":
                PerformUltimate();
                break;
            default:
                UnityEngine.Debug.LogError("Invalid action: " + action);
                break;
        }
    }

    protected override void Start()
    {
        base.Start(); // Call the base class's Start method first
        if (team != Enums.Team.ALLIES) transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
        InitializeSubsystems();
        // Additional behavior specific to the derived class's Start method
    }

    bool trackingTarget = false;
    protected override void Update()
    {
        base.Update(); // Call the base class's Update method first
        // Additional behavior specific to the derived class's Update method
        //MoveToTheLeft();
        FindTargets();
        MeleeDetection();
        if (CanAttack())
        {
            if (inMeleeRange == true) AttackMode("melee");
            else AttackMode("ranged");
        }
        if (mainTarget != null)
        {
            if (!trackingTarget) {
                trackingTarget = true;
                StartCoroutine(TrackingMainTarget());
            }
            FireAtMainTarget();
        }

    }

    private void FindTargets()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, enemyData.attackRange);

        foreach(Collider2D collider in colliders)
        {
            if (collider != null)
            {
                EnemyProfiling enemyProfiling = collider.gameObject.GetComponent<EnemyProfiling>();
                CharacterProfiling characterProfiling = collider.gameObject.GetComponent<CharacterProfiling>();

                if (enemyProfiling != null && enemyProfiling.team != team) {
                    if (mainTarget == null)
                    {
                        mainTarget = collider.gameObject;
                        SetMainWeaponTargets(mainTarget);
                    }
                }
                if (characterProfiling != null && characterProfiling.team != team) {
                    if (mainTarget == null)
                    {
                        mainTarget = collider.gameObject;
                        SetMainWeaponTargets(mainTarget);
                    }
                }
            }
        }

    }

    private void SetMainWeaponTargets(GameObject target)
    {
        foreach(GameObject beamfarer in beamfarerTurrets)
        {
            if (beamfarer != null)
            {
                NiexpieriaBeamfarer subsystemManager = beamfarer.GetComponent<NiexpieriaBeamfarer>();
                if (subsystemManager != null)
                {
                    subsystemManager.SetTarget(target);
                    UnityEngine.Debug.Log("Set main target");
                }
            }
        }
    }

    private IEnumerator TrackingMainTarget()
    {
        while (true)
        {
            if (mainTarget != null)
            {
                float distance = Vector3.Distance(transform.position, mainTarget.transform.position);

                // Check if the mainTarget is out of range (you can replace the threshold distance)
                if (distance > enemyData.attackRange)
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

    private void FireAtMainTarget()
    {
        foreach (GameObject beamfarer in beamfarerTurrets)
        {
            if (beamfarer != null)
            {
                NiexpieriaBeamfarer subsystemManager = beamfarer.GetComponent<NiexpieriaBeamfarer>();
                if (subsystemManager != null)
                {
                    subsystemManager.FireUponLoad();
                    UnityEngine.Debug.Log("Set main target");
                }
            }
        }
    }

    private void InitializeSubsystems()
    {
        calibratingSubsystems.Add(mainTurret);
        foreach (GameObject pulseTurret in pulseTurrets) calibratingSubsystems.Add(pulseTurret);
        foreach (GameObject beamfarerTurret in beamfarerTurrets) calibratingSubsystems.Add(beamfarerTurret);

        foreach (GameObject subsystem in calibratingSubsystems)
        {
            if (subsystem != null) {
                SubsystemProfiling module = subsystem.GetComponent<SubsystemProfiling>();
                if (module == null) continue;

                module.SetTeam(team);
                module.SetParent(gameObject);
            }
        }
    }

    private bool CanAttack()
    {
        return Time.time - lastAttackTime >= base.enemyData.attackCooldown;
    }

    private bool IsEnemy(GameObject other)
    {
        return other.CompareTag("Player");
    }

    private void MeleeDetection()
    {
        Collider2D[] detectionColliders = Physics2D.OverlapCircleAll(transform.position, meleeDetectionRange);
        List<Collider2D> enemies = new List<Collider2D>();

        foreach (Collider2D detectionCollider in detectionColliders)
        {
            if (IsEnemy(detectionCollider.gameObject))
            {
                enemies.Add(detectionCollider);
                inMeleeRange = true;
            }
        }

        if (enemies.Count <= 0)
        {
            inMeleeRange = false;
        }

        //UnityEngine.Debug.Log("Melee Detection Script is Active, Status of Melee: " + inMeleeRange);
    }

    private void AttackMode(string attackType)
    {
        Collider2D[] hitColliders;

        if (attackType == "melee")
        {
            hitColliders = Physics2D.OverlapCircleAll(transform.position, meleeDetectionRange);
        }
        else
        {
            hitColliders = Physics2D.OverlapCircleAll(transform.position, base.enemyData.attackRange);
        }


        
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

    private void PerformAttack()
    {
        // Define your attack logic here
        // For example, reduce enemy health or apply status effects
        UnityEngine.Debug.Log("" + " performs an attack!");
    }

    private void PerformRanged()
    {
        StartCoroutine(PerformRangedWithDelay());

        //UnityEngine.Debug.Log("" + " performs ranged attack!");
    }

    private void PerformHeal()
    {
        // Define your healing logic here
        // For example, increase Health or remove status effects
        UnityEngine.Debug.Log("" + " performs a heal!");
    }

    private void PerformUltimate()
    {
        // Define your ultimate ability logic here
        // For example, deal massive damage or apply powerful effects
        UnityEngine.Debug.Log("" + " performs their ultimate ability!");
    }

    private IEnumerator PerformRangedWithDelay()
    {
        List<GameObject> targets = team != Enums.Team.ALLIES ? GameObject.Find("Main Camera")?.GetComponent<PartyController>().spawnedPrefabs : new List<GameObject>();
        if (targets != null && targets.Count > 0)
        {
            targets.RemoveAll(target => target == null);
            targets.RemoveAll(target => target != null && target.GetComponent<CharacterProfiling>() != null && target.GetComponent<CharacterProfiling>().isDead);
        }

        Collider2D[] detected = Physics2D.OverlapCircleAll(transform.position, enemyData.attackRange);
        foreach (Collider2D hitCollider in detected)
        {
            if (targets.Contains(hitCollider.gameObject)) continue;
            if (IsEnemy(hitCollider.gameObject))
            {
                targets.Add(hitCollider.gameObject);
            }
        }

        float launchInterval = 0.3f;

        foreach (Transform missileLaunch in launchPorts)
        {
            if (missileLaunch != null)
            {
                GameObject missilee = Instantiate(missile.prefab, missileLaunch.position, Quaternion.identity);
                GenericActions.MissileAttack(missile, team, enemyData, missilee, Vector3.left, gameObject);
                if (targets.Count > 0)
                {
                    int random = UnityEngine.Random.Range(0, targets.Count);
                    if (targets.ToArray()[random] != null) missilee.GetComponent<MissileController>().SetTarget(targets.ToArray()[random]);
                }
                if (GlobalSoundManager.IsWithinRange(gameObject)) GlobalSoundManager.GlobalSoundPlayer.PlayOneShot(missileLaunchClip, 0.6f);

                // Wait for the specified launch interval before the next iteration.
                yield return new WaitForSeconds(launchInterval);
            }
        }

        yield break;
        
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        foreach (GameObject subsystem in calibratingSubsystems)
        {
            if (subsystem != null)
            Destroy(subsystem);
        }
    }
}
