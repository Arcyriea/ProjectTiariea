using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultEnemy : EnemyProfiling
{
    public BulletProperties bullet;

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
        MeleeDetection();
        if (CanAttack())
        {
            UnityEngine.Debug.Log("The enemy is able to attack");
            if (inMeleeRange == true) AttackMode("melee"); 
            else AttackMode("ranged");
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

        foreach (Collider2D hitCollider in hitColliders)
        {
            if (IsEnemy(hitCollider.gameObject))
            {
                lastAttackTime = Time.time;
                // Perform melee/ranged attack on the enemy
                switch (attackType)
                {
                    case "melee":
                        PerformAttack();
                        break;
                    case "ranged":
                        PerformRanged();
                        break;
                }
                // Implement your attack logic here
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
        List<GameObject> targets = GameObject.Find("Main Camera")?.GetComponent<PartyController>().spawnedPrefabs;
        if (targets != null && targets.Count > 0) targets.RemoveAll(target => target.GetComponent<CharacterProfiling>().isDead);

        GameObject bulletGO = Instantiate(bullet.bulletPrefab, transform.position, Quaternion.identity);

        // Get the BulletController component from the instantiated bullet
        BulletController bulletController = bulletGO.GetComponent<BulletController>();

        if (bulletController != null)
        {
            // Initialize the bullet's properties

            bulletController.Initialize(gameObject.tag, enemyData.attackDamage, enemyData.attackRange / enemyData.projectileSpeed, enemyData.projectileSpeed, enemyData.splashRadius);

            // Set the direction of the bullet (adjust this based on your game logic)
            Vector3 bulletDirection = Vector3.right; // Example direction
            bulletController.SetDirection(bulletDirection);
        }
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
