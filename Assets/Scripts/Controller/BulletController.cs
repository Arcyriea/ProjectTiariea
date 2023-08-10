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

    public void Initialize(string tag, float damage, float lifetime, float speed, float explodeRadius)
    {
        sideTag = tag;
        this.damage = damage;
        this.lifetime = lifetime;
        this.speed = speed;
        this.explodeRadius = explodeRadius;
    }

    public void SetDirection(Vector3 direction)
    {
        this.direction = direction;
    }

    private void Start() { 
        gameObject.tag = sideTag;
    }

    private void Update()
    {
        // Move the bullet based on the direction and speed
        transform.position += direction * speed * Time.deltaTime;

        // Decrease the lifetime of the bullet
        lifetime -= Time.deltaTime;
        if (lifetime <= 0f)
        {
            // Destroy the bullet when its lifetime expires
            Destroy(gameObject);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {

    }

    // Add any other bullet behavior and collision handling here
}
