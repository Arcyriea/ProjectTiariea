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
    public float speed;
    public float splashRadius;
    public float detectionRadius;
    public bool boomerang;
    public bool penetrates;
    public int penetrationCount;
    public float penetrationStrength;
}
