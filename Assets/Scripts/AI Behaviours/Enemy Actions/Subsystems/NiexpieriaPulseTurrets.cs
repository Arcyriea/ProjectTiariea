using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NiexpieriaPulseTurrets : SubsystemProfiling
{
    public float turnSpeed;
    public Transform target { get; private set; }
    private Quaternion originalRotation;
    protected override void Start()
    {
        base.Start();
        originalRotation = transform.rotation;
        StartCoroutine(ScanTargets());
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

    protected override void PerformRanged()
    {
        throw new System.NotImplementedException();
    }

    private IEnumerator ScanTargets()
    {

        yield return new WaitForSeconds(1.0f);
    }
}
