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
    private Vector3 direction;
    private bool interceptCollision;
    private bool penetrates;

    public float growRate { get; private set; }

    public Enums.Team team { get; private set; }

    protected BoxCollider2D boxCollider;
    protected CapsuleCollider2D capsuleCollider;
    protected PolygonCollider2D polygonCollider;

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

    private void Update()
    {
        UpdateGrowRate(); 
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

    void OnTriggerEnter2D(Collider2D collision)
    {
        BulletController otherBullet = collision.gameObject.GetComponent<BulletController>();
        CharacterProfiling character = collision.gameObject.GetComponent<CharacterProfiling>();
        EnemyProfiling entity = collision.gameObject.GetComponent<EnemyProfiling>();

        UnityEngine.Debug.Log("Bullet Collision Triggered");


        if (otherBullet != null && interceptCollision == true)
        {
            // Check the team affiliation of the other bullet
            if (otherBullet.team != team)
            {
                // Handle the collision based on team affiliation
                HandleBulletCollision(otherBullet);
                UnityEngine.Debug.Log("otherBullet Triggered");
            }
        }

        if (character != null)
        {
            if (character.team != team)
            {
                UnityEngine.Debug.Log("characterBulletCollision Triggered");
                if (character.Health > damage)
                {
                    if (penetrates != true)
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
                    damage -= remainingHealth;
                }
            }
        }

        if (entity != null)
        {
            if (entity.team != team)
            {
                UnityEngine.Debug.Log("entityBulletCollision Triggered");
                if (entity.Health > damage)
                {
                    if (penetrates != true)
                    {
                        entity.TakeDamage(damage);
                        Destroy(gameObject);
                    }
                    else
                    {
                        if (entity.punctured != true) entity.TakeDamage(damage);
                        entity.punctured = true;
                    }
                }
                else
                {
                    float remainingHealth = entity.Health;
                    entity.TakeDamage(damage);
                    damage -= remainingHealth;
                }
            }
        }
    }

    void UpdateGrowRate()
    {
        if (growRate > 0)
        {
            transform.localScale = new Vector3(transform.localScale.x * growRate, transform.localScale.y * growRate, transform.localScale.z);
            damage *= growRate;
            if (polygonCollider != null)
            {
                Vector2[] newVertices = polygonCollider.points; // Get the current vertices

                float wideningFactor = growRate; 

                for (int i = 0; i < newVertices.Length; i++)
                {
                    newVertices[i].x *= wideningFactor;
                    newVertices[i].y *= wideningFactor;
                }

                polygonCollider.SetPath(0, newVertices);
            }

            if (capsuleCollider != null)
            {
                Vector2 newSize = capsuleCollider.size;
                newSize.x *= growRate;
                newSize.y *= growRate;
                capsuleCollider.size = newSize;
            }

            if (boxCollider != null)
            {
                Vector2 newSize = boxCollider.size;
                newSize.x *= growRate; // Change the factor as needed
                newSize.y *= growRate;
                boxCollider.size = newSize;
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

    // Add any other bullet behavior and collision handling here
    public void DecrementDamage(float damage)
    {
        this.damage -= damage;
    }

    private void RetrieveColliders()
    {
        if (gameObject.GetComponent<PolygonCollider2D>() != null)
        {
            polygonCollider = gameObject.GetComponent<PolygonCollider2D>();
        }
        if (gameObject.GetComponent<CapsuleCollider2D>() != null)
        {
            capsuleCollider = gameObject.GetComponent<CapsuleCollider2D>();
        }

        if (gameObject.GetComponent<BoxCollider2D>() != null)
        {
            boxCollider = gameObject.GetComponent<BoxCollider2D>();
        }
    }
}
