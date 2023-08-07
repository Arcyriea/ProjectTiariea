using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Bullet")]
public class BulletProperties : ScriptableObject
{
    public float damage;
    public float lifetime;
    public float speed;
    public float explodeRadius;
    public float size;
    public float growRate;
    public GameObject bulletPrefab;
}
