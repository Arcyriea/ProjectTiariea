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
    public Enums.Team team { get; private set; }

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
    }

    private void Update()
    {
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

    void OnCollisionEnter2D(Collision2D collision)
    {
        BulletController otherBullet = collision.gameObject.GetComponent<BulletController>();
        CharacterProfiling character = collision.gameObject.GetComponent<CharacterProfiling>();
        EnemyProfiling entity = collision.gameObject.GetComponent<EnemyProfiling>();

        if (otherBullet != null && interceptCollision == true)
        {
            // Check the team affiliation of the other bullet
            if (otherBullet.team != team)
            {
                // Handle the collision based on team affiliation
                HandleBulletCollision(otherBullet);
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
                        if (character.punctured != true) character.TakeDamage(damage);
                        character.punctured = true;
                    }
                }
                else damage -= character.Health;
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
                        if (entity.punctured != true) entity.TakeDamage(damage);
                        entity.punctured = true;
                    }
                }
                else damage -= entity.Health;
            }
        }
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
}
