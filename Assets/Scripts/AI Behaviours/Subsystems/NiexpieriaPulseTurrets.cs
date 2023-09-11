using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NiexpieriaPulseTurrets : SubsystemProfiling
{
    public float turnSpeed;
    public BulletProperties bullet;
    public Transform target { get; private set; }
    private Quaternion originalRotation;
    private float lastAttackTime;
    protected override void Start()
    {
        base.Start();
        originalRotation = transform.rotation;
        StartCoroutine(ScanTargets());
        lastAttackTime = Time.time;
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        FireOnTarget();
        PointToTarget();
    }

    private void FireOnTarget()
    {
        if (Time.time - lastAttackTime >= subsystemData.attackCooldown && target != null)
        {
            PerformRanged();
            lastAttackTime = Time.time;
        }
    }
    private void PointToTarget()
    {
        if (target != null)
        {
            Vector3 targetDirection = target.transform.position - transform.position;
            float targetAngle = Mathf.Atan2(targetDirection.y, targetDirection.x) * Mathf.Rad2Deg;
            targetAngle += 180f;

            // Smoothly rotate the turret towards the target only on the Z-axis
            Quaternion targetRotation = Quaternion.Euler(0, 0, targetAngle);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, turnSpeed * Time.deltaTime);
        }
        else
        {
            UnityEngine.Debug.Log("PD Target set to null");
            transform.rotation = Quaternion.RotateTowards(transform.rotation, originalRotation, turnSpeed * Time.deltaTime);
        }
    }

    protected override void PerformRanged()
    {
        float firingAngle = team == Enums.Team.ALLIES ? 180 : 0f;
        Vector3 firingDirection = -gameObject.transform.right; // The original firing direction

        // Create a quaternion to represent the desired rotation
        Quaternion rotationQuaternion = Quaternion.Euler(0f, 0f, firingAngle);

        // Rotate the firing direction vector
        Vector3 rotatedDirection = rotationQuaternion * firingDirection;

        Vector3 offset = new Vector3(team == Enums.Team.ALLIES ? transform.position.x +  5f : transform.position.x - 5f, transform.position.y, transform.position.z);
        GenericActions.BulletAttack(bullet, team, subsystemData, Instantiate(bullet.bulletPrefab, offset, Quaternion.identity), rotatedDirection);
    }

    private IEnumerator ScanTargets()
    {
        

        if (target == null)
        {
            List<GameObject> targets = new List<GameObject>();
            Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, subsystemData.attackRange);
            foreach (Collider2D collider in colliders)
            {
                if (collider != null)
                {
                    EnemyProfiling enemyProfiling = collider.gameObject.GetComponent<EnemyProfiling>();
                    CharacterProfiling characterProfiling = collider.gameObject.GetComponent<CharacterProfiling>();

                    if (enemyProfiling != null && enemyProfiling.team != team)
                    {
                        targets.Add(collider.gameObject);
                    }
                    if (characterProfiling != null && characterProfiling.team != team && !characterProfiling.isDead)
                    {
                        targets.Add(collider.gameObject);
                    }
                }
            }

            int random = Random.Range(0, targets.Count);
            target = targets.ToArray()[random].transform;
        } 
        else
        {
            float distance = Vector3.Distance(transform.position, target.position);

            EnemyProfiling entityProfiling = target.gameObject.GetComponent<EnemyProfiling>();
            CharacterProfiling characterProfiling = target.gameObject.GetComponent<CharacterProfiling>();

            if (target.gameObject == null) target = null;

            if (entityProfiling != null)
            {
                if (entityProfiling.team == team || entityProfiling.Health <= 0)
                {
                    target = null;
                }
            }

            if (characterProfiling != null)
            {
                if (characterProfiling.isDead || characterProfiling.Health <= 0 || characterProfiling.team == team)
                {
                    target = null; // Set mainTarget to null if out of range
                }
            }
            // Check if the mainTarget is out of range (you can replace the threshold distance)
            if (distance > subsystemData.attackRange)
            {
                target = null; // Set mainTarget to null if out of range
            }
        }

        yield return null;
    }
}
