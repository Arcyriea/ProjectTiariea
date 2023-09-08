using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NiexpieriaBeamfarer : SubsystemProfiling
{
    public float turnSpeed;
    public float bulletStreamInterval;
    public GameObject target { get; private set; }
    public BulletProperties bulletStream;
    public AudioClip BeamAudio;
    public AudioClip chargeAudio;
    private StreamInitializer streamInitializer = null;
    private float nextAttackCooldown = 0;

    private Quaternion originalRotation;
    private Vector3 firingPoint = new Vector3(-5f, 0, 0);
    // Start is called before the first frame update

    private void Awake()
    {
        target = null;
    }
    protected override void Start()
    {
        base.Start();
        originalRotation = transform.rotation;
        nextAttackCooldown = Time.time + subsystemData.attackCooldown;
        if (streamInitializer == null)
        {
            streamInitializer = gameObject.AddComponent<StreamInitializer>();
            streamInitializer.SetOriginTransform(transform);
        }
        StartCoroutine(PointToTarget());
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        
    }

    private IEnumerator PointToTarget()
    {
        while (true)
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
                transform.rotation = Quaternion.RotateTowards(transform.rotation, originalRotation, turnSpeed * Time.deltaTime);
            }
            yield return null;
        }
    }

    public void FireUponLoad()
    {
        if (Time.time >= nextAttackCooldown)
        {
            PerformRanged();
            nextAttackCooldown = Time.time + subsystemData.attackCooldown;
            UnityEngine.Debug.Log("Firing at target");
        }
    }

    protected override void PerformRanged()
    {
        StartCoroutine(GenericActions.ChargingUp(
            streamInitializer.StreamBulletAttack(bulletStream, team, subsystemData, bulletStream.bulletPrefab, firingPoint, 0, bulletStreamInterval, BeamAudio.length, null),
            chargeAudio.length, chargeAudio, BeamAudio)
            );
        UnityEngine.Debug.Log("Starting Coroutine for Charging up");
    }

    public void SetTarget(GameObject target) { 
        this.target = target;
    }
}
