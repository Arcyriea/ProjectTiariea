using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TiarieanFrigate : BattleshipProfiling
{
    // Start is called before the first frame update
    public GameObject mainBeamTurret;
    public Transform mainPhantomEmitter;
    public BulletProperties mainOrbProperty;
    public AudioClip orbLaunchAudio;

    protected override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        FindTargets();
        if (CanAttack())
        {
            AttackMode("ranged");
        }
        if (mainTarget != null)
        {
            if (!trackingTarget)
            {
                trackingTarget = true;
                StartCoroutine(TrackingMainTarget());
            }
            FireAtMainTarget();
        }
    }

    protected override void InitializeSubsystems()
    {
        calibratingSubsystems.Add(mainBeamTurret);

        base.InitializeSubsystems();
    }

    protected override void SetMainWeaponTargets(GameObject target)
    {
            if (mainBeamTurret != null)
            {
                TiarieanBeamTurret subsystemManager = mainBeamTurret.GetComponent<TiarieanBeamTurret>();
                if (subsystemManager != null && subsystemManager.target != mainTarget)
                {
                    subsystemManager.SetTarget(target);
                    UnityEngine.Debug.Log("Set main target");
                }
            }
    }

    protected override void PerformRanged()
    {
        StartCoroutine(PerformRangedWithDelay());

        //UnityEngine.Debug.Log("" + " performs ranged attack!");
    }

    protected override void FireAtMainTarget()
    {

            if (mainBeamTurret != null)
            {
                TiarieanBeamTurret subsystemManager = mainBeamTurret.GetComponent<TiarieanBeamTurret>();
                if (subsystemManager != null)
                {
                    subsystemManager.FireUponLoad();
                    UnityEngine.Debug.Log("Set main target");
                }
            }

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

        float launchInterval = 0.3f;
        int orbAmounts = 6;

        
        if (mainPhantomEmitter != null)
        {
           
            if (targets.Count > 0)
            {
                for (int i = 0; i < orbAmounts; i++)
                {
                    int random = UnityEngine.Random.Range(0, targets.Count);
                    if (targets.ToArray()[random] != null)
                    {
                        GameObject orb = Instantiate(mainOrbProperty.bulletPrefab, mainPhantomEmitter.position, Quaternion.identity);
                        Vector3 directionToTarget = targets[random].transform.position - mainPhantomEmitter.position;

                        // Normalize the direction vector to get a unit vector
                        Vector3 normalizedDirection = directionToTarget.normalized;

                        GenericActions.BulletAttack(mainOrbProperty, team, battleshipProperty, orb, normalizedDirection);
                        if (GlobalSoundManager.IsWithinRange(gameObject) && orbLaunchAudio != null) GlobalSoundManager.GlobalSoundPlayer.PlayOneShot(orbLaunchAudio, 0.6f);
                        yield return new WaitForSeconds(launchInterval);
                    }
                    
                }
            }
            //

            // Wait for the specified launch interval before the next iteration.
            
        }


        yield break;

    }
}
