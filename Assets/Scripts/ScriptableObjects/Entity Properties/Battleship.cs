using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Battleship")]
[Serializable]
public class Battleship : ScriptableObject
{
    // Enemy identification
    public string batteshipId;
    public string batteshipName;
    public GameObject batteshipPrefab;
    public Enums.ClassType batteshipClass;
    [SerializeField]
    public Enums.Team team;

    // Enemy stats and behavior attributes
    public float maximumHealth;
    public float maximumShield;
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

    public Battleship(GameObject prefab, string id, string name, Sprite sprite, Animator animator, float health, float range, float damage, float splash, float projSpeed, float cooldown)
    {
        batteshipPrefab = prefab;
        batteshipId = id;
        batteshipName = name;
        maximumHealth = health;
        attackRange = range;
        attackDamage = damage;
        splashRadius = splash;
        projectileSpeed = projSpeed;
        attackCooldown = cooldown;
    }
}
