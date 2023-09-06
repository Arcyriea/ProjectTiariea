using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    private string sideTag;
    private float damage;
    private float lifetime;
    private float speed;
    private float explodeRadius;
    public Vector3 direction { get; private set; }
    private bool interceptCollision;
    private bool penetrates;
    private int penetrationCounts = 2;
    private bool penetrated = false;
    public float growRate { get; private set; }

    public Enums.Team team { get; private set; }

    protected BoxCollider2D boxCollider;
    protected CapsuleCollider2D capsuleCollider;
    protected PolygonCollider2D polygonCollider;

    private GameObject currentBullet;

    public void Initialize(string tag, Enums.Team team, float damage, float lifetime, float speed, float explodeRadius, bool intercept, bool penetrate)
    {
        sideTag = tag;
        this.team = team;
        this.damage = damage;
        this.lifetime = lifetime;
        this.speed = speed;
        this.explodeRadius = explodeRadius;
        interceptCollision = intercept;
        penetrates = penetrate;
    }

    public void SetDirection(Vector3 direction)
    {
        this.direction = direction;
    }

    public void SetTeam(Enums.Team team)
    {
        this.team = team;
    }

    private void Start()
    {
        gameObject.tag = "Bullet";
        RetrieveColliders();
    }

    bool resettingPenetrated = false;
    private void Update()
    {
        currentlyInsides.RemoveAll(inside => inside == null);
        if (penetrates)
        {
            if (penetrated && !resettingPenetrated)
            {
                resettingPenetrated = true;
                StartCoroutine(ResetPenetration());
            }
        }
        //UpdateGrowRate();
        // Move the bullet based on the direction and speed
        transform.position += direction * speed * Time.deltaTime;

        // Decrease the lifetime of the bullet
        lifetime -= Time.deltaTime;
        if (lifetime <= 0f || damage < 0f)
        {
            // Destroy the bullet when its lifetime expires or damage is drained out
            Destroy(gameObject);
        }
    }

    IEnumerator ResetPenetration()
    {
        yield return new WaitForSeconds(1f); 
        penetrated = false;
        resettingPenetrated = false;
        yield break;
    }

    List<GameObject> currentlyInsides = new List<GameObject>();

    private void OnTriggerExit2D(Collider2D collision)
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

        if (otherBullet != null && interceptCollision == true)
        {
            // Check the team affiliation of the other bullet
            if (otherBullet.team != team)
            {
                // Handle the collision based on team affiliation
                HandleBulletCollision(otherBullet);
                
            }
        }

        if (missile != null && interceptCollision == true)
        {
            // Check the team affiliation of the other bullet
            if (missile.team != team)
            {
                // Handle the collision based on team affiliation
                HandleMissileCollision(missile);
                
            }
        }

        if (character != null)
        {
            if (character.team != team)
            {
                
                if (character.Health > damage)
                {
                    if (penetrates != true)
                    {
                        character.TakeDamage(damage);
                        Destroy(gameObject);
                    }
                    else
                    {
                        if (!penetrated && !currentlyInsides.Contains(collision.gameObject))
                        {
                            character.TakeDamage(damage);
                            penetrationCounts -= 1;
                            if (penetrationCounts <= 0) Destroy(gameObject);
                            penetrated = true;
                            currentlyInsides.Add(collision.gameObject);
                        }
                    }
                }
                else
                {
                    float remainingHealth = character.Health;
                    character.TakeDamage(damage);
                    damage -= remainingHealth;
                }
                if (character.Health <= 0 && team == Enums.Team.ALLIES) PartyController.score += 1000;
            }
        }

        if (entity != null)
        {
            if (entity.team != team)
            {
                
                if (entity.Health > damage)
                {
                    if (penetrates != true)
                    {
                        entity.TakeDamage(damage);
                        Destroy(gameObject);
                    }
                    else
                    {
                        if (!penetrated && !currentlyInsides.Contains(collision.gameObject))
                        {
                            entity.TakeDamage(damage);
                            penetrationCounts -= 1;
                            if (penetrationCounts <= 0) Destroy(gameObject);
                            penetrated = true;
                            currentlyInsides.Add(collision.gameObject);
                        }
                        
                    }
                }
                else
                {
                    float remainingHealth = entity.Health;
                    entity.TakeDamage(damage);
                    damage -= remainingHealth;
                }
                if (entity.Health <= 0 && team == Enums.Team.ALLIES) PartyController.score += 100;
            }
        }

        if (subsystem != null)
        {
            if (subsystem.team != team)
            {
                if (subsystem.Health > damage)
                {
                    if (penetrates != true)
                    {
                        subsystem.TakeDamage(damage);
                        Destroy(gameObject);
                    }
                    else
                    {
                        if (!penetrated && !currentlyInsides.Contains(collision.gameObject))
                        {
                            subsystem.TakeDamage(damage);
                            penetrationCounts -= 1;
                            if (penetrationCounts <= 0) Destroy(gameObject);
                            penetrated = true;
                            currentlyInsides.Add(collision.gameObject);
                        }
                    }
                }
                else
                {
                    float remainingHealth = subsystem.Health;
                    subsystem.TakeDamage(damage);
                    damage -= remainingHealth;
                }
            }
        }
    }

    public void SetGrowRate(float growRate)
    {
        if (growRate > 0)
            this.growRate = 1f + growRate;
        else this.growRate = 0;
    }

    private void HandleBulletCollision(BulletController otherBullet)
    {
        float otherDamage = otherBullet.damage;
        float selfDamage = damage;
        otherBullet.DecrementDamage(selfDamage);
        DecrementDamage(otherDamage);
    }

    private void HandleMissileCollision(MissileController missile)
    {
        float missileHealth = missile.health;
        float selfDamage = damage;
        missile.TakeDamage(selfDamage);
        damage -= missileHealth;
    }

    // Add any other bullet behavior and collision handling here
    public void DecrementDamage(float damage)
    {
        this.damage -= damage;
    }

    private void RetrieveColliders()
    {
        Collider2D[] colliders = gameObject.GetComponents<Collider2D>();

        foreach (Collider2D collider in colliders)
        {
            if (collider is PolygonCollider2D polygon)
            {
                polygonCollider = polygon;
            }
            if (collider is CapsuleCollider2D capsule)
            {
                capsuleCollider = capsule;
            }

            if (collider is BoxCollider2D box)
            {
                boxCollider = box;
            }
        }
    }
}
