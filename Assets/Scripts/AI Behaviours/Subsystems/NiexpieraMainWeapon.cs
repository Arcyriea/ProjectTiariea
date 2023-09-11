using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NiexpieraMainWeapon : SubsystemProfiling
{
    public Transform BarrageDirection;

    public Transform[] LaunchTubes;
    public MissileProperties missileProperties;
    public Vector3 target { get; private set; }
    public AudioClip audioClip;
    private StreamInitializer streamInitializer = null;
    private float lastAttackTime;
    bool targetsNearby = false;
    protected override void Start()
    {
        base.Start();
        streamInitializer = gameObject.AddComponent<StreamInitializer>();
        streamInitializer.SetOriginTransform(transform);
        lastAttackTime = Time.time;
        StartCoroutine(ScanTargets());
    }
    bool launching = false;
    protected override void Update()
    {
        base.Update();
        if (Time.time >= lastAttackTime && targetsNearby)
        {
            if (!launching)
            {
                StartCoroutine(performLaunch());
                launching = true;
            }
        }
    }

    private IEnumerator performLaunch()
    {
        
        foreach (Transform tube in LaunchTubes)
        {
            int missileAmounts = 6;
            for (int i = 0; i < missileAmounts; i++) { 
                GenericActions.MissileAttack(missileProperties, team, subsystemData, Instantiate(missileProperties.prefab, tube.position, Quaternion.identity), (BarrageDirection.position - tube.position).normalized, gameObject);
                UnityEngine.Debug.Log("Launching Missiles");
                yield return new WaitForSeconds(0.6f);
            }
            
            yield return new WaitForSeconds(4f);
        }
            
        
        launching = false;
        lastAttackTime = Time.time;
        yield break;
    }

    private IEnumerator ScanTargets()
    {
            List<GameObject> targets = new List<GameObject>();
            Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, subsystemData.attackRange);
            foreach (Collider2D collider in colliders)
            {
                if (collider != null)
                {
                    EnemyProfiling enemyProfiling = collider.gameObject.GetComponent<EnemyProfiling>();
                    CharacterProfiling characterProfiling = collider.gameObject.GetComponent<CharacterProfiling>();
                    BattleshipProfiling battleshipProfiling = collider.gameObject.GetComponent<BattleshipProfiling>();

                    if (enemyProfiling != null && enemyProfiling.team != team)
                    {
                        targets.Add(collider.gameObject);
                    }
                    if (characterProfiling != null && characterProfiling.team != team && !characterProfiling.isDead)
                    {
                        targets.Add(collider.gameObject);
                    }
                    if(battleshipProfiling != null && battleshipProfiling.team != team)
                    {
                        targets.Add(collider.gameObject);
                    }
                }
            }
            if (targets.Count > 0) { targetsNearby = true; } else { targetsNearby = false; }

        

        yield return null;
    }
}
