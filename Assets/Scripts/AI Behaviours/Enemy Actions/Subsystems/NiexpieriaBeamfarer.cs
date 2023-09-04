using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NiexpieriaBeamfarer : SubsystemProfiling
{
    public float turnSpeed;
    public float attackTime;
    public float bulletStreamInterval;
    public Transform target { get; private set; }
    public BulletProperties bulletStream;
    public AudioClip BeamAudio;
    private StreamInitializer streamInitializer = null;
    private float nextAttackCooldown = 0;

    private Quaternion originalRotation;
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        originalRotation = transform.rotation;
        nextAttackCooldown = Time.time + attackTime;
        if (streamInitializer == null)
        {
            streamInitializer = new StreamInitializer();
            streamInitializer.SetOriginTransform(transform);
        }
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        PointToTarget();
    }

    private void PointToTarget()
    {
        if (target != null)
        {
            Vector3 targetDirection = target.position - transform.position;
            Quaternion targetRotation = Quaternion.LookRotation(targetDirection);

            // Smoothly rotate the turret towards the target
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, turnSpeed * Time.deltaTime);
        } 
        else
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, originalRotation, turnSpeed * Time.deltaTime);
        }
    }

    public void FireUponLoad()
    {
        if (Time.time >= nextAttackCooldown)
        {
            PerformRanged();
            nextAttackCooldown = Time.time + attackTime;
        }
    }

    protected override void PerformRanged()
    {
        StartCoroutine(streamInitializer.StreamBulletAttack(bulletStream, team, enemyData, bulletStream.bulletPrefab, Vector3.right, bulletStreamInterval, BeamAudio.length, BeamAudio));
    }

    public void SetTarget(Transform target) { 
        this.target = target;
    }

    public void ResetTarget()
    {
        target = null;
    }
}
