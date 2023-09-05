using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Bullet")]
public class BulletProperties : ScriptableObject
{
    public float damage;
    public float speed;
    public float size;
    public float growRate;
    public GameObject bulletPrefab;
    public bool intercept;
    public bool penetrate;
}
