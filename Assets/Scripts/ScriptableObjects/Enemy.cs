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

    // Enemy stats and behavior attributes
    public float maximumHealth;
    public float attackRange;
    public float attackDamage;
    public float attackCooldown;
    // Add more attributes as needed to represent enemy behaviors and stats

    // Method to handle enemy behavior (e.g., AI logic, attack patterns, etc.)
    public void EnemyBehavior()
    {
        // Implement enemy behavior here
    }

    public Enemy(GameObject prefab, string id, string name, Sprite sprite, Animator animator, float health, float range, float damage, float cooldown)
    {
        enemyPrefab = prefab;
        enemyId = id;
        enemyName = name;   
        maximumHealth = health;
        attackRange = range;
        attackDamage = damage;
        attackCooldown = cooldown;
    }
}

public static class Enemies
{
    public static List<Enemy> enemies = new List<Enemy>();


    private static Sprite LoadSpriteByGUID(string spriteGUID)
    {
        string path = UnityEditor.AssetDatabase.GUIDToAssetPath(spriteGUID);
        return UnityEditor.AssetDatabase.LoadAssetAtPath<Sprite>(path);
    }

    private static Animator LoadAnimatorByGUID(string animGUID)
    {
        string path = UnityEditor.AssetDatabase.GUIDToAssetPath(animGUID);
        return UnityEditor.AssetDatabase.LoadAssetAtPath<Animator>(path);
    }
}
