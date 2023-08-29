using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Enums;

public class MissileController : MonoBehaviour
{
    private Object parentEntity;
    private MissileProperties properties;
    private Transform parentTransform;
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

    public float maxRange { get; private set; }

    public Vector3 direction { get; private set; }
    public Team team { get; private set; }
    private float shortestDistance = Mathf.Infinity;
    private bool isBoomerang = false;

    void Update()
    {
        if (health <= 0) Destroy(gameObject);

        if (lifeTime <= 0)
        {
            if (properties.boomerang)
            {
                target = null;
                
                MoveTowardsTarget();

                if (Vector3.Distance(transform.position, target.position) <= 0) Destroy(gameObject);
                UnityEngine.Debug.Log("Ran into LifeTime.Boomerang check of Update");
            }
            else
            {
                Destroy(gameObject);
            }
            UnityEngine.Debug.Log("Ran into LifeTime check of Update");
        }

        RotatePerpetually();

        if (properties.homing)
        {
            if (hasTarget)
            {
                if (lifeTime > 0) MoveTowardsTarget();

                if (properties.boomerang && Vector3.Distance(transform.position, target.position) > maxRange)
                {
                    isBoomerang = true;
                    target = parentTransform; // Set target as the missile's origin
                    velocity *= -1;     // Reverse the velocity
                }
            }
            else
            {
                if (lifeTime > 0)
                {
                    MoveUntilOutOfLifeTime();
                    SearchForTarget();
                }  
            }
        } 
        else
        {
            if (lifeTime > 0) MoveUntilOutOfLifeTime();
        }

        lifeTime -= Time.deltaTime;
    }

    public void Initialize(string tag, Object obj, Team team, MissileProperties missileProperties, Transform parentTrans)
    {
        parentEntity = obj;
        gameObject.tag = tag;
        this.team = team;
        this.properties = missileProperties;
        isBoomerang = properties.boomerang;
        parentTransform = parentTrans;

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

        

        health = properties.health == 0 ? parentHealth / 10f : properties.health;
        damage = properties.damage == 0 ? parentDamage : properties.damage;
        acceleration = properties.acceleration;
        maxSpeed = properties.maxSpeed;
        rotationSpeed = properties.rotationSpeed;
        splashRadius = properties.splashRadius;
        detectionRadius = properties.detectionRadius;
        lifeTime = properties.lifeTime == 0 ? parentRange / properties.maxSpeed : properties.lifeTime;
        maxRange = parentRange;
        // Initialize missile properties
        // For example: health, damage, speed, etc.

        // Search for the nearest target
        if (properties.homing) SearchForTarget();
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

    void MoveUntilOutOfLifeTime()
    {
        Vector3 acceleration = transform.right * this.acceleration * Time.deltaTime;
        velocity += acceleration;

        velocity = Vector3.ClampMagnitude(velocity, maxSpeed);

        // Update the missile's position based on its velocity
        transform.position += velocity * Time.deltaTime;
    }

    void MoveTowardsTarget()
    {
        if (target == null)
        {
            if (isBoomerang)
            {
                target = parentTransform; // Set target as the missile's origin
                velocity *= -1;     // Reverse the velocity
            }
            else
            {
                hasTarget = false;
                return;
            }
        }

        Vector3 directionToTarget = (target.position - transform.position).normalized;

        Vector3 acceleration = directionToTarget * this.acceleration * Time.deltaTime;
        velocity += acceleration;

        velocity = Vector3.ClampMagnitude(velocity, maxSpeed);

        if (!properties.perpetualOscilliation)
        {
            Quaternion targetRotation = Quaternion.LookRotation(Vector3.forward, directionToTarget);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }

        transform.position += velocity * Time.deltaTime;

        // Check if the missile hit the target or reached a certain distance
        // Apply damage to the target and destroy the missile if necessary
    }

    void RotatePerpetually()
    {
        if (properties.perpetualOscilliation)
        {
            transform.Rotate(Vector3.forward, rotationSpeed * Time.deltaTime);
        }
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        BulletController otherBullet = collision.gameObject.GetComponent<BulletController>();
        MissileController missile = collision.gameObject.GetComponent<MissileController>();
        CharacterProfiling character = collision.gameObject.GetComponent<CharacterProfiling>();
        EnemyProfiling entity = collision.gameObject.GetComponent<EnemyProfiling>();

        //UnityEngine.Debug.Log("Bullet Collision Triggered");

        if (missile != null && properties.intercept == true)
        {
            float totalDamage = properties.hullDamage ? damage + health : damage;
            if (missile.team != team)
            {
                if (properties.penetrates != true)
                {
                    missile.TakeDamage(totalDamage);
                    if (!properties.boomerang) Destroy(gameObject);
                    else
                    {
                        target = null;
                        MoveTowardsTarget();
                        //Destroy game object if returned to parent transform
                    }
                }
                else
                {
                    missile.TakeDamage(totalDamage);
                    if (!properties.boomerang) health -= missile.damage;
                }
                
            }
        }

        if (character != null)
        {
            if (character.team != team)
            {
                //UnityEngine.Debug.Log("characterBulletCollision Triggered");
                if (character.Health > damage)
                {
                    if (properties.penetrates != true)
                    {
                        character.TakeDamage(damage);
                        Destroy(gameObject);
                    }
                    else
                    {
                        if (character.punctured != true) character.TakeDamage(damage);
                        character.punctured = true;
                    }
                }
                else
                {
                    float remainingHealth = character.Health;
                    character.TakeDamage(damage);
                    if (!properties.boomerang) health -= remainingHealth;
                }
            }
        }

        if (entity != null)
        {
            if (entity.team != team)
            {
                //UnityEngine.Debug.Log("entityBulletCollision Triggered");
                if (entity.Health > damage)
                {
                    if (properties.penetrates != true)
                    {
                        entity.TakeDamage(damage);
                        Destroy(gameObject);
                    }
                    else
                    {
                        if (entity.punctured != true) entity.TakeDamage(damage);
                        //UnityEngine.Debug.Log("Entity Taken Damage, remaining Health:" + entity.Health);
                        entity.punctured = true;
                    }
                }
                else
                {
                    float remainingHealth = entity.Health;
                    entity.TakeDamage(damage);
                    if (!properties.boomerang) health -= remainingHealth;
                }
            }
        }
    }
    // Other methods like damage application, collision handling, etc.
}
