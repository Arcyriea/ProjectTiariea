using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Enemy")]
public class Enemy : ScriptableObject
{
    // Enemy identification
    public string enemyId;
    public string enemyName;
    public GameObject enemyPrefab;
    public Enums.ClassType enemyClass;
    [SerializeField]
    public Enums.Team team;

    // Enemy stats and behavior attributes
    public float maximumHealth;
    public float attackRange;
    public float attackDamage;
    public float splashRadius;
    public float projectileSpeed;
    public float attackCooldown;
    // Add more attributes as needed to represent enemy behaviors and stats

    // Method to handle enemy behavior (e.g., AI logic, attack patterns, etc.)
    public virtual void EnemyBehavior()
    {
        // Implement enemy behavior here
    }

    public Enemy(GameObject prefab, string id, string name, Sprite sprite, Animator animator, float health, float range, float damage, float splash, float projSpeed, float cooldown)
    {
        enemyPrefab = prefab;
        enemyId = id;
        enemyName = name;   
        maximumHealth = health;
        attackRange = range;
        attackDamage = damage;
        splashRadius = splash;
        projectileSpeed = projSpeed;
        attackCooldown = cooldown;
    }
}
