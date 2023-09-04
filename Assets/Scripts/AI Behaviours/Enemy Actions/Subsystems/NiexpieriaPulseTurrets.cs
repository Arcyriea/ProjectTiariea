using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NiexpieriaPulseTurrets : SubsystemProfiling
{
    public float turnSpeed;
    public Transform target { get; private set; }
    protected override void Start()
    {
        base.Start();
        StartCoroutine(ScanTargets());
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();

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
