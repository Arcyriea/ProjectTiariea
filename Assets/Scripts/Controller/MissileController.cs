using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Enums;

public class MissileController : MonoBehaviour
{
    private Object parentEntity;
    private MissileProperties properties;
    private Transform target;
    private Vector3 velocity;
    private bool hasTarget;
    //stat
    public float health { get; private set; }
    public float damage { get; private set; }
    public float acceleration { get; private set; }
    public float maxSpeed { get; private set; }
    public float rotationSpeed { get; private set; }
    public float splashRadius { get; private set; }
    public float detectionRadius { get; private set; }
    public float lifeTime { get; private set; }

    public Vector3 direction { get; private set; }
    public Team team { get; private set; }
    private float shortestDistance = Mathf.Infinity;
    
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

    public void Initialize(string tag, Object obj, Team team, MissileProperties missileProperties)
    {
        parentEntity = obj;
        gameObject.tag = tag;
        this.team = team;
        this.properties = missileProperties;

        float parentHealth = 0;
        float parentDamage = 0;
        float parentRange = 0;
        if(parentEntity is Character chara)
        {
            parentHealth = chara.maximumHealth;
            parentDamage = chara.rangedDamage;
            parentRange = chara.shootingRange;
        } else if (parentEntity is Enemy enemy)
        {
            parentHealth = enemy.maximumHealth;
            parentDamage = enemy.attackDamage;
            parentRange = enemy.attackRange;
        } else
        {
            UnityEngine.Debug.LogError("Invalid Object assigned for Initialize method of MissileController");
            return;
        }

        

        health = missileProperties.health == 0 ? parentHealth / 10f : missileProperties.health;
        damage = missileProperties.damage == 0 ? parentDamage : missileProperties.damage;
        acceleration = missileProperties.acceleration;
        maxSpeed = missileProperties.maxSpeed;
        rotationSpeed = missileProperties.rotationSpeed;
        splashRadius = missileProperties.splashRadius;
        detectionRadius = missileProperties.detectionRadius;
        lifeTime = missileProperties.lifeTime == 0 ? parentRange / missileProperties.maxSpeed : missileProperties.lifeTime;

        // Initialize missile properties
        // For example: health, damage, speed, etc.

        // Search for the nearest target
        SearchForTarget();
    }

    public void SetDirection(Vector3 direction)
    {
        this.direction = direction;
    }

    void SearchForTarget()
    {
        // Implement your target searching logic here
        // For example, find the nearest enemy based on tags or other criteria
        // Set 'target' and 'hasTarget' appropriately

        Collider2D[] detectionSphere = Physics2D.OverlapCircleAll(transform.position, detectionRadius);

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

        Vector3 acceleration = directionToTarget * this.acceleration * Time.deltaTime;
        velocity += acceleration;

        velocity = Vector3.ClampMagnitude(velocity, maxSpeed);

        Quaternion targetRotation = Quaternion.LookRotation(Vector3.forward, directionToTarget);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

        transform.position += velocity * Time.deltaTime;

        // Check if the missile hit the target or reached a certain distance
        // Apply damage to the target and destroy the missile if necessary
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        
    }
    // Other methods like damage application, collision handling, etc.
}
