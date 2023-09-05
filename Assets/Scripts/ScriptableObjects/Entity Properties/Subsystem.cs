using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Subsystem")]
[Serializable]
public class Subsystem : ScriptableObject
{
    // Enemy identification
    public string subsystemId;
    public string subsystemName;
    public Enums.ClassType enemyClass;
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

}
