using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Missile")]
public class MissileProperties : ScriptableObject
{
    public GameObject prefab;
    public float health;
    public float damage;
    public float acceleration;
    public float maxSpeed;
    public float rotationSpeed;
    public float splashRadius;
    public float detectionRadius;
    public float lifeTime;
    public bool boomerang;
    public bool penetrates;
    public int penetrationCount;
    public float penetrationStrength;
}