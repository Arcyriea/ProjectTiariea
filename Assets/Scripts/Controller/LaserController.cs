using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class LaserController : MonoBehaviour
{
    public LineRenderer lineRenderer;
    public Transform laserPosition;
    public BeamProperties beamProperties;

    private Character character;
    private Enemy enemy;
    private Enums.Team team;

    private float damagePerSecond; // Damage per second
    // Start is called before the first frame update
    void Start()
    {
        lineRenderer.enabled = false;
        if (gameObject.GetComponent<CharacterProfiling>() != null)
        {
            character = gameObject.GetComponent<CharacterProfiling>().character;
            team = character.team;
        }
        if (gameObject.GetComponent<EnemyProfiling>() != null)
        {
            enemy = gameObject.GetComponent<EnemyProfiling>().enemyData;
            team = enemy.team;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (beamProperties == null) return;
        RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, transform.right);
        hits = FilterHits(hits).ToArray();
        lineRenderer.SetPosition(0, laserPosition.position);

        if (character != null)
        {      
            ExcecutePhysic(hits, beamProperties.penetrate, character.shootingRange);
        }
        else if (enemy != null)
        {
            ExcecutePhysic(hits, beamProperties.penetrate, enemy.attackRange);
        }
    }

    void ExcecutePhysic(RaycastHit2D[] hits, bool penetrate, float range)
    {
        

        if (hits.Length > 0)
        {
            switch (penetrate)
            {
                case true:
                    lineRenderer.SetPosition(1, transform.right * range);
                    foreach (RaycastHit2D hit in hits)
                    {
                        ApplyDamage(hit.collider.gameObject, damagePerSecond);
                    }
                    break;
                case false:
                    lineRenderer.SetPosition(1, hits[0].point);
                    ApplyDamage(hits[0].collider.gameObject, damagePerSecond);
                    break;
            }
        }
        else
        {
            lineRenderer.SetPosition(1, transform.right * range);
            ApplyDamage(null, damagePerSecond);
        }
    }

    void ApplyDamage(GameObject target, float damage)
    {
        if (target == null) return;

        CharacterProfiling characterProfiling = target.GetComponent<CharacterProfiling>();
        EnemyProfiling enemyProfiling = target.GetComponent<EnemyProfiling>();
        BulletController bullets = beamProperties.intercept ? target.GetComponent<BulletController>() : null;

        if (characterProfiling != null)
        {
            // Apply damage to the character
            characterProfiling.TakeDamage(damage * Time.deltaTime);
        }
        else if (enemyProfiling != null)
        {
            // Apply damage to the enemy
            enemyProfiling.TakeDamage(damage * Time.deltaTime);
        }
        else if (bullets != null)
        {
            bullets.DecrementDamage(damage * Time.deltaTime);
        }
    }

    public void SetActive(bool active)
    {
        lineRenderer.enabled = active;
    }

    IEnumerable<RaycastHit2D> FilterHits(RaycastHit2D[] hits)
    {
        if (!beamProperties.intercept)
        {
            return hits.Where(hit => (hit.collider.GetComponent<EnemyProfiling>() != null 
            && hit.collider.GetComponent<EnemyProfiling>().team != team)
            || (hit.collider.GetComponent<CharacterProfiling>() != null
            && hit.collider.GetComponent<CharacterProfiling>().team != team));
        } else
        {
            return hits.Where(hit => (hit.collider.GetComponent<EnemyProfiling>() != null
            && hit.collider.GetComponent<EnemyProfiling>().team != team)
            || (hit.collider.GetComponent<CharacterProfiling>() != null
            && hit.collider.GetComponent<CharacterProfiling>().team != team)
            || (hit.collider.GetComponent<BulletController>() != null 
            && hit.collider.GetComponent<BulletController>().team != team));
        }
        
    }
}
