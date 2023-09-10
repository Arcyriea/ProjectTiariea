using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;


public class NiexpieriaBoss : BattleshipProfiling
{
    public MissileProperties missile;
    public AudioClip missileLaunchClip;

    public GameObject mainTurret;
    public GameObject[] pulseTurrets;
    public GameObject[] beamfarerTurrets;
    public Transform[] launchPorts;
    public Transform[] torpedoPorts;

    protected override void Start()
    {
        base.Start(); // Call the base class's Start method first
        // Additional behavior specific to the derived class's Start method
    }

    
    protected override void Update()
    {
        base.Update(); // Call the base class's Update method first
        // Additional behavior specific to the derived class's Update method
        //MoveToTheLeft();
        FindTargets();
        if (CanAttack())
        {
            AttackMode("ranged");
        }
        if (mainTarget != null)
        {
            if (!trackingTarget) {
                trackingTarget = true;
                StartCoroutine(TrackingMainTarget());
            }
            FireAtMainTarget();
        }

    }

    protected override void SetMainWeaponTargets(GameObject target)
    {
        foreach (GameObject beamfarer in beamfarerTurrets)
        {
            if (beamfarer != null)
            {
                NiexpieriaBeamfarer subsystemManager = beamfarer.GetComponent<NiexpieriaBeamfarer>();
                if (subsystemManager != null && subsystemManager.target != mainTarget)
                {
                    subsystemManager.SetTarget(target);
                    UnityEngine.Debug.Log("Set main target");
                }
            }
        }
    }

    protected override void FireAtMainTarget()
    {
        foreach (GameObject beamfarer in beamfarerTurrets)
        {
            if (beamfarer != null)
            {
                NiexpieriaBeamfarer subsystemManager = beamfarer.GetComponent<NiexpieriaBeamfarer>();
                if (subsystemManager != null)
                {
                    subsystemManager.FireUponLoad();
                    UnityEngine.Debug.Log("Set main target");
                }
            }
        }
    }

    protected override void InitializeSubsystems()
    {
        calibratingSubsystems.Add(mainTurret);
        foreach (GameObject pulseTurret in pulseTurrets) calibratingSubsystems.Add(pulseTurret);
        foreach (GameObject beamfarerTurret in beamfarerTurrets) calibratingSubsystems.Add(beamfarerTurret);

        base.InitializeSubsystems();
    }

    protected override void PerformAttack()
    {
        // Define your attack logic here
        // For example, reduce enemy health or apply status effects
        UnityEngine.Debug.Log("" + " performs an attack!");
    }

    protected override void PerformRanged()
    {
        StartCoroutine(PerformRangedWithDelay());

        //UnityEngine.Debug.Log("" + " performs ranged attack!");
    }

    protected override void PerformHeal()
    {
        // Define your healing logic here
        // For example, increase Health or remove status effects
        UnityEngine.Debug.Log("" + " performs a heal!");
    }

    protected override void PerformUltimate()
    {
        // Define your ultimate ability logic here
        // For example, deal massive damage or apply powerful effects
        UnityEngine.Debug.Log("" + " performs their ultimate ability!");
    }

    private IEnumerator PerformRangedWithDelay()
    {
        List<GameObject> targets = team != Enums.Team.ALLIES ? GameObject.Find("Main Camera")?.GetComponent<PartyController>().spawnedPrefabs : new List<GameObject>();
        if (targets != null && targets.Count > 0)
        {
            targets.RemoveAll(target => target == null);
            targets.RemoveAll(target => target != null && target.GetComponent<CharacterProfiling>() != null && target.GetComponent<CharacterProfiling>().isDead);
        }

        Collider2D[] detected = Physics2D.OverlapCircleAll(transform.position, battleshipProperty.attackRange);
        foreach (Collider2D hitCollider in detected)
        {
            if (targets.Contains(hitCollider.gameObject)) continue;
            if (IsEnemy(hitCollider.gameObject))
            {
                targets.Add(hitCollider.gameObject);
            }
        }

        if (!missile.destroySubsystems) targets.RemoveAll(target => target.GetComponent<SubsystemProfiling>() != null);

        float launchInterval = 0.3f;

        foreach (Transform missileLaunch in launchPorts)
        {
            if (missileLaunch != null)
            {
                GameObject missilee = Instantiate(missile.prefab, missileLaunch.position, Quaternion.identity);
                GenericActions.MissileAttack(missile, team, battleshipProperty, missilee, Vector3.left, gameObject);
                if (targets.Count > 0)
                {
                    int random = UnityEngine.Random.Range(0, targets.Count);
                    if (targets.ToArray()[random] != null) missilee.GetComponent<MissileController>().SetTarget(targets.ToArray()[random]);
                }
                if (GlobalSoundManager.IsWithinRange(gameObject)) GlobalSoundManager.GlobalSoundPlayer.PlayOneShot(missileLaunchClip, 0.6f);

                // Wait for the specified launch interval before the next iteration.
                yield return new WaitForSeconds(launchInterval);
            }
        }

        yield break;
        
    }

}
