using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Enums;

public class MissileController : MonoBehaviour
{
    public GameObject parentGameObject { get; private set; }
    public Object parentEntity { get; private set; }
    public MissileProperties properties { get; private set; }
    private Rigidbody2D rb;


    public Transform target { get; private set; }
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
    private int penetrationCounts;

    private void Awake()
    {
        target = null;
    }

    private void Start()
    {
        penetrationCounts = properties.penetrationCount == 0 ? 2 : properties.penetrationCount;
        rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            if (properties.coldLaunchSpeed > 0)
            {
                rb.AddForce(transform.up * properties.coldLaunchSpeed, ForceMode2D.Impulse);
            }
        }
        StartCoroutine(HandlingOutOfLifetime());
    }

    void Update()
    {
        if (health <= 0) Destroy(gameObject);
        currentlyInsides.RemoveAll(inside => inside == null);

        if (target == null) MoveUntilOutOfLifeTime(); 
        RotatePerpetually();

        if (properties.homing)
        {
            if (hasTarget)
            {
                if (lifeTime > 0) MoveTowardsTarget();

                if (properties.boomerang && Vector3.Distance(transform.position, target.position) > maxRange)
                {
                    isBoomerang = true;
                    target = parentGameObject.transform; // Set target as the missile's origin
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

        if (lifeTime <= 0)
        {
            if (properties.boomerang)
            {
                target = parentGameObject.transform;
                MoveTowardsTarget();
            }
        }

        lifeTime -= Time.deltaTime;
    }

    private IEnumerator HandlingOutOfLifetime()
    {
        while (true)
        {
            if (target != null)
            {
                CharacterProfiling targetedCharacter = target.gameObject.GetComponent<CharacterProfiling>();
                if (targetedCharacter != null)
                {
                    if (targetedCharacter.isDead) {
                        target = null;
                        MoveTowardsTarget();
                    }
                }
            }

            if (lifeTime <= 0)
            {
                if (properties.boomerang)
                {
                    Destroy(gameObject, 10f);
                    yield break;
                }
                else
                {
                    Destroy(gameObject);
                }
            }
            yield return null;
        }
    }

    public void Initialize(string tag, Object obj, Team team, MissileProperties missileProperties, GameObject parentTrans)
    {
        parentEntity = obj;
        parentGameObject = parentTrans;
        //gameObject.tag = tag;
        this.team = team;
        this.properties = missileProperties;
        isBoomerang = properties.boomerang;
       

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
        } else if (parentEntity is Battleship battleship)
        {
            parentHealth = battleship.maximumHealth;
            parentDamage = battleship.attackDamage;
            parentRange = battleship.attackRange;
        } else if (parentEntity is Subsystem subsystem)
        {
            parentHealth = subsystem.maximumHealth;
            parentDamage = subsystem.attackDamage;
            parentRange = subsystem.attackRange;
        }
        
        else
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
        //UnityEngine.Debug.Log("Missile lifetime: " +lifeTime);
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
            SubsystemProfiling subsystem = enmity.GetComponent<SubsystemProfiling>();

            if (entity != null)
            {
                if (entity.team != team)
                {
                    FindNearestTarget(enmity);
                }
            }

            else if (chara != null)
            {
                if (chara.team != team && !chara.isDead)
                {
                    FindNearestTarget(enmity);
                }
            }

            else if (subsystem != null && properties.destroySubsystems)
            {
                if (subsystem.team != team)
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
                target = parentGameObject.transform; // Set target as the missile's origin
                //velocity *= -1;     // Reverse the velocity
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
            targetRotation *= Quaternion.Euler(0, 0, 90);
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

    public void SetTarget(GameObject gameObject)
    {
        if (gameObject != null)
        target = gameObject.transform;
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
    }

    List<GameObject> currentlyInsides = new List<GameObject>();
    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision != null)
        {
            if (currentlyInsides.Contains(collision.gameObject)) currentlyInsides.Remove(collision.gameObject);
        }
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        BulletController otherBullet = collision.gameObject.GetComponent<BulletController>();
        MissileController missile = collision.gameObject.GetComponent<MissileController>();
        CharacterProfiling character = collision.gameObject.GetComponent<CharacterProfiling>();
        EnemyProfiling entity = collision.gameObject.GetComponent<EnemyProfiling>();
        SubsystemProfiling subsystem = collision.gameObject.GetComponent<SubsystemProfiling>();
        BattleshipProfiling battleship = collision.gameObject.GetComponent<BattleshipProfiling>();

        //UnityEngine.Debug.Log("Bullet Collision Triggered");

        if (collision.gameObject == parentGameObject && properties.boomerang && target == parentGameObject.transform) Destroy(gameObject);

        
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
                        target = parentGameObject.transform;
                        MoveTowardsTarget();
                        //Destroy game object if returned to parent transform
                       
                    }
                }
                else
                {
                    missile.TakeDamage(totalDamage);
                    if (!properties.boomerang)
                    {
                        penetrationCounts -= 1;
                        if (penetrationCounts <= 0) Destroy(gameObject);
                    }
                    else health -= missile.damage;
                }
                
            }
        }

        if (subsystem != null && properties.destroySubsystems)
        {
            if (subsystem.team != team)
            {
                //UnityEngine.Debug.Log("entityBulletCollision Triggered");
                if (subsystem.Health > damage)
                {
                    if (properties.penetrates != true)
                    {
                        subsystem.TakeDamage(damage);
                        if (!properties.boomerang) Destroy(gameObject);
                        else
                        {
                            target = parentGameObject.transform;
                            MoveTowardsTarget();
                        }
                    }
                    else
                    {
                        if (!currentlyInsides.Contains(collision.gameObject))
                        {
                            subsystem.TakeDamage(damage);
                            penetrationCounts -= 1;
                            if (penetrationCounts <= 0) Destroy(gameObject);
                            currentlyInsides.Add(collision.gameObject);
                        }
                    }
                }
                else
                {
                    float remainingHealth = subsystem.Health;
                    subsystem.TakeDamage(damage);
                    if (!properties.boomerang) health -= remainingHealth;
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
                        if (!properties.boomerang) Destroy(gameObject);
                        else
                        {
                            target = parentGameObject.transform;
                            MoveTowardsTarget();
                        }
                    }
                    else
                    {
                        if (!currentlyInsides.Contains(collision.gameObject))
                        {
                            character.TakeDamage(damage);
                            penetrationCounts -= 1;
                            if (penetrationCounts <= 0) Destroy(gameObject);
                            currentlyInsides.Add(collision.gameObject);
                        }
                    }
                }
                else
                {
                    float remainingHealth = character.Health;
                    character.TakeDamage(damage);

                    if (!properties.boomerang) health -= remainingHealth;
                }
                if (character.Health <= 0 && team == Team.ALLIES) PartyController.score += 100;
                if (character.isDead)
                {
                    if (!properties.boomerang)
                    {
                        target = null;
                        MoveUntilOutOfLifeTime();
                        SearchForTarget();
                    }
                    else
                    {
                        target = parentGameObject.transform;
                        MoveTowardsTarget();
                    }
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
                        if (!properties.boomerang) Destroy(gameObject);
                        else
                        {
                            target = parentGameObject.transform;
                            MoveTowardsTarget();
                        }
                    }
                    else
                    {
                        if (!currentlyInsides.Contains(collision.gameObject))
                        {
                            entity.TakeDamage(damage);
                            penetrationCounts -= 1;
                            if (penetrationCounts <= 0) Destroy(gameObject);
                            currentlyInsides.Add(collision.gameObject);
                        }
                    }
                }
                else
                {
                    float remainingHealth = entity.Health;
                    entity.TakeDamage(damage);
                    if (!properties.boomerang) health -= remainingHealth;
                }
                if (entity.Health <= 0 && team == Team.ALLIES) PartyController.score += 100;
            }
        }

        if (battleship != null)
        {
            if (battleship.team != team)
            {
                //UnityEngine.Debug.Log("entityBulletCollision Triggered");
                if (battleship.Health > damage)
                {
                    if (properties.penetrates != true)
                    {
                        battleship.TakeDamage(damage);
                        if (!properties.boomerang) Destroy(gameObject);
                        else
                        {
                            target = parentGameObject.transform;
                            MoveTowardsTarget();
                        }
                    }
                    else
                    {
                        if (!currentlyInsides.Contains(collision.gameObject))
                        {
                            battleship.TakeDamage(damage);
                            penetrationCounts -= 1;
                            if (penetrationCounts <= 0) Destroy(gameObject);
                            currentlyInsides.Add(collision.gameObject);
                        }
                    }
                }
                else
                {
                    float remainingHealth = battleship.Health;
                    battleship.TakeDamage(damage);
                    if (!properties.boomerang) health -= remainingHealth;
                }
                if (battleship.Health <= 0 && team == Team.ALLIES) PartyController.score += 200;
            }
        }
    }
    // Other methods like damage application, collision handling, etc.
}
