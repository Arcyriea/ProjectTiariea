using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class NiexpieriaBoss : EnemyProfiling
{
    public MissileProperties missile;
    public AudioClip missileLaunchClip;

    public GameObject mainTurret;
    public Enemy[] turrets;
    public Transform[] turretPositions;
    public Transform[] launchPorts;
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
        //MoveToTheLeft();
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
        StartCoroutine(PerformRangedWithDelay());

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

    private IEnumerator PerformRangedWithDelay()
    {
        List<GameObject> targets = GameObject.Find("Main Camera")?.GetComponent<PartyController>().spawnedPrefabs;
        if (targets != null && targets.Count > 0) targets.RemoveAll(target => target.GetComponent<CharacterProfiling>().isDead);

        float launchInterval = 0.3f;

        foreach (Transform missileLaunch in launchPorts)
        {
            if (missileLaunch != null)
            {
                GameObject missilee = Instantiate(missile.prefab, missileLaunch.position, Quaternion.identity);
                GenericActions.MissileAttack(missile, team, enemyData, missilee, Vector3.down, gameObject);
                int random = UnityEngine.Random.Range(0, targets.Count);
                missilee.GetComponent<MissileController>().SetTarget(targets.ToArray()[random]);
                if (GlobalSoundManager.IsWithinRange(gameObject)) GlobalSoundManager.GlobalSoundPlayer.PlayOneShot(missileLaunchClip, 0.6f);

                // Wait for the specified launch interval before the next iteration.
                yield return new WaitForSeconds(launchInterval);
            }
        }

        yield break;
        
    }

}