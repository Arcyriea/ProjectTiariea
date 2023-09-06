using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NiexpieraMainWeapon : SubsystemProfiling
{
    public Transform BarrageDirection;

    public Transform[] LaunchTubes;
    public Vector3 target { get; private set; }
    protected override void Start()
    {
        base.Start();
       
    }

    protected override void Update()
    {
        base.Update();
    }
}
