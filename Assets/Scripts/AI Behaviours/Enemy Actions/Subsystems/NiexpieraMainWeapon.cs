using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NiexpieraMainWeapon : SubsystemProfiling
{
    public Enemy overrideData;

    public Vector3 target { get; private set; }
    protected override void Start()
    {
        base.Start();
        enemyData = overrideData;
        Health = enemyData.maximumHealth;
        Shield = enemyData.maximumShield;
    }

    protected override void Update()
    {
        base.Update();
    }
}
