using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.TextCore.Text;
using static UnityEngine.GraphicsBuffer;

public class MissileLauncherSquare : EnemyProfiling
{
    public MissileProperties missile;
    public AudioClip missileLaunchClip;
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
        // Additional behavior specific to the derived class's Start method
    }

    protected override void Update()
    {
        base.Update(); // Call the base class's Update method first
        // Additional behavior specific to the derived class's Update method
        MoveToTheLeft();
        MeleeDetection();
        if (CanAttack())
        {
            if (inMeleeRange == true) AttackMode("melee");
            else AttackMode("ranged");
        }

    }

    private void MoveToTheLeft()
    {
        transform.position = new Vector3(transform.position.x - 0.01f, transform.position.y, transform.position.z);
    }

    private bool CanAttack()
    {
        return Time.time - lastAttackTime >= base.enemyData.attackCooldown;
    }

    private bool IsEnemy(GameObject other)
    {
        EnemyProfiling entity = other.GetComponent<EnemyProfiling>();
        CharacterProfiling character = other.GetComponent<CharacterProfiling>();
        SubsystemProfiling subsystem = other.GetComponent <SubsystemProfiling>();

        if (entity != null) { if (entity.team != team) return true; }
        if (character != null) { if (character.team != team) return true; }
        if (subsystem != null) { if (subsystem.team != team) return true; }

        return false;
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

        UnityEngine.Debug.Log("Melee Detection Script is Active, Status of Melee: " + inMeleeRange);
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


        GameObject[] missiles =
        {
            Instantiate(missile.prefab, transform.position, Quaternion.identity),
            Instantiate(missile.prefab, transform.position, Quaternion.identity),
            Instantiate(missile.prefab, transform.position, Quaternion.identity),
            Instantiate(missile.prefab, transform.position, Quaternion.identity)
        };

        Vector3[] vectorDirections = { Vector3.right, Vector3.left, Vector3.up, Vector3.down };
        // Get the BulletController component from the instantiated bullet
        int count = 0;
        foreach (GameObject spawn in missiles)
        {
            GenericActions.MissileAttack(missile, team, enemyData, spawn, vectorDirections[count], gameObject);
            if (targets.Count > 0)
            {
                int random = UnityEngine.Random.Range(0, targets.Count);
                spawn.GetComponent<MissileController>().SetTarget(targets.ToArray()[random]);
            }
            count++;
        }

        if (GlobalSoundManager.IsWithinRange(gameObject)) GlobalSoundManager.GlobalSoundPlayer.PlayOneShot(missileLaunchClip, 0.6f);
        UnityEngine.Debug.Log("" + " performs ranged attack!");
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
}
