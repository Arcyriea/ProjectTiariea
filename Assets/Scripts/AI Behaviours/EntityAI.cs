using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class EntityAI : MonoBehaviour
{
    public float detectionRange = 5f; // Adjust the detection range as needed
    public float attackCooldown = 1f; // Time between attacks

    private EnemyProfiling entityProfiling;
    private bool inMeleeRange = false;
    private float lastAttackTime = 0f;

    private void Start()
    {
        entityProfiling = GetComponent<EnemyProfiling>();
    }

    private void Update()
    {
        if (!inMeleeRange && CanAttack())
        {
            PerformAttack("melee");
        }
    }

    private bool CanAttack()
    {
        return Time.time - lastAttackTime >= attackCooldown;
    }

    private void PerformAttack(string attackType)
    {
        Collider2D[] hitColliders;
        Collider2D[] detectionColliders = Physics2D.OverlapCircleAll(transform.position, detectionRange);
        List<Collider2D> enemies = new List<Collider2D>();

        foreach (Collider2D detectionCollider in detectionColliders)
        {
            if (IsEnemy(detectionCollider.gameObject))
            {
                enemies.Add(detectionCollider);
                inMeleeRange = true;
            }
        }

        if (enemies.Count <= 0 && inMeleeRange)
        {
            inMeleeRange = false;
            return;
        }

        if (attackType == "melee")
        {
            hitColliders = detectionColliders;
        }
        else
        {
            hitColliders = Physics2D.OverlapCircleAll(transform.position, entityProfiling.enemyData.attackRange);
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
                        UnityEngine.Debug.Log("Performing melee attack on enemy: " + hitCollider.gameObject.name);
                        break;
                    case "ranged":
                        UnityEngine.Debug.Log("Performing ranged attack on enemy: " + hitCollider.gameObject.name);
                        break;
                }
                // Implement your attack logic here
            }
        }
    }

    private bool IsEnemy(GameObject other)
    {
        return other.CompareTag("Enemy");
    }
}
