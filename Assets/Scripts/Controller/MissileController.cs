using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Enums;

public class MissileController : MonoBehaviour
{
    public MissileProperties missileProperties;

    private Transform target;
    private Vector3 velocity;
    private bool hasTarget;
    public Team team;
    private float shortestDistance = Mathf.Infinity;
    void Start()
    {
        Initialize();
    }

    void Update()
    {
        if (hasTarget)
        {
            MoveTowardsTarget();
        }
        else
        {
            SearchForTarget();
        }
    }

    void Initialize()
    {
        if (missileProperties == null || missileProperties.prefab == null)
        {
            Debug.LogError("Missile properties not set properly.");
            return;
        }

        // Initialize missile properties
        // For example: health, damage, speed, etc.

        // Search for the nearest target
        SearchForTarget();
    }

    void SearchForTarget()
    {
        // Implement your target searching logic here
        // For example, find the nearest enemy based on tags or other criteria
        // Set 'target' and 'hasTarget' appropriately

        Collider2D[] detectionSphere = Physics2D.OverlapCircleAll(transform.position, missileProperties.detectionRadius);

        foreach (Collider2D enmity in detectionSphere)
        {
            EnemyProfiling entity = enmity.GetComponent<EnemyProfiling>();
            CharacterProfiling chara = enmity.GetComponent<CharacterProfiling>();

            if (entity != null)
            {
                if (entity.team != team)
                {
                    FindNearestTarget(enmity);
                }
            }

            else if (chara != null)
            {
                if (chara.team != team)
                {
                    FindNearestTarget(enmity);
                }
            }
        }

    }

    void FindNearestTarget(Collider2D enmity)
    {
        float distanceToTarget = Vector3.Distance(transform.position, enmity.transform.position);
        if (distanceToTarget < shortestDistance)
        {
            shortestDistance = distanceToTarget;
            target = enmity.transform;
            hasTarget = true;
        }
    }

    void MoveTowardsTarget()
    {
        if (target == null)
        {
            hasTarget = false;
            return;
        }

        Vector3 directionToTarget = (target.position - transform.position).normalized;

        Vector3 acceleration = directionToTarget * missileProperties.acceleration * Time.deltaTime;
        velocity += acceleration;

        velocity = Vector3.ClampMagnitude(velocity, missileProperties.maxSpeed);

        Quaternion targetRotation = Quaternion.LookRotation(Vector3.forward, directionToTarget);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, missileProperties.rotationSpeed * Time.deltaTime);

        transform.position += velocity * Time.deltaTime;

        // Check if the missile hit the target or reached a certain distance
        // Apply damage to the target and destroy the missile if necessary
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        
    }
    // Other methods like damage application, collision handling, etc.
}
