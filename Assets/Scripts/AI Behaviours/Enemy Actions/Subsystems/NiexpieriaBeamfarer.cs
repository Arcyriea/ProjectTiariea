using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NiexpieriaBeamfarer : SubsystemProfiling
{
    public float turnSpeed;
    public Transform target { get; private set; }

    private Quaternion originalRotation;
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        originalRotation = transform.rotation;
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

    public void FireUponTarget()
    {
        if (target != null) PerformRanged();
    }

    public void SetTarget(Transform target) { 
        this.target = target;
    }

    public void ResetTarget()
    {
        target = null;
    }
}
