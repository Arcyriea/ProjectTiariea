using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class ShiariakyiaActions : CharacterProfiling
{
    public BulletProperties bullet;
    public MissileProperties ringBoomerang;

    public MissileProperties homingRays;
    public Transform[] meleeAttackPoints;

    public GameObject[] dartMinions;
    private Transform[] dartMinionTransforms;
    private float orbitRadius = 6f;
    private float dartRangedAttackTime;

    private float homingRaysDelay;

    public AudioClip dartRangedAttackClip;
    public AudioClip boomerangThrowClip;
    public override void CharacterAction(string action)
    {
        GenericActions.ExecuteAction(this, action);
    }

    protected override void Start()
    {
        base.Start();
        bullet.damage = character.rangedDamage / dartMinions.Length;
        ringBoomerang.damage = character.rangedDamage * 2;
        homingRays.damage = character.rangedDamage / dartMinions.Length * 1.5f;
        InitializeDarts();
        UltimateMeter = character.fireRate * 10;

    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        DartOrbit();
        if (base.moveToMouse.selected == true)
        {
            if (Input.GetKey(KeyCode.Space))
            {
                SyncRangedAttacks();
            }

            if (Input.GetMouseButton(1))
            {
                SyncMeleeAttacks();
            }
        } 
        else
        {
            ControlAI();
        }
    }

    protected override void ControlAI()
    {
        foreach(Transform melee in meleeAttackPoints)
        {
            if (commandAI.MeleeDetection(melee))
            {
                commandAI.ReportStatus(true);
                return;
            }
        }
        commandAI.ReportStatus(false);
    }
    public override void SyncRangedAttacks()
    {
        DartAttacks();
        if (!animator.GetBool("Ultimate")) CharacterAction("ranged");
    }

    public override void SyncMeleeAttacks()
    {
        if (!animator.GetBool("Ultimate")) CharacterAction("attack");
    }

    private void InitializeDarts()
    {
        dartMinionTransforms = new Transform[dartMinions.Length];
        for (int i = 0; i < dartMinions.Length; i++)
        {
            dartMinions[i] = Instantiate(dartMinions[i]);
            float angle = 2 * Mathf.PI * i / dartMinions.Length;
            Vector3 offset = new Vector3(Mathf.Cos(angle) * orbitRadius,
                                        Mathf.Sin(angle) * orbitRadius,
                                        0f);
            dartMinions[i].transform.position = transform.position + offset;
            dartMinionTransforms[i] = dartMinions[i].transform;
        }
    }

    private void DartOrbit()
    {
        // Update dartMinions' positions for formation
        float orbitSpeed = 3f; // Adjust the speed of orbit

        for (int i = 0; i < dartMinions.Length; i++)
        {
            float angle = (Time.time * orbitSpeed) + (2 * Mathf.PI * i / dartMinions.Length);
            Vector3 offset = new Vector3(Mathf.Cos(angle) * orbitRadius,
                                         Mathf.Sin(angle) * orbitRadius,
                                         0f);
            dartMinionTransforms[i].position = transform.position + offset;
        }
    }

    public void DartAttacks()
    {
        if (Time.time - dartRangedAttackTime >= character.fireRate / (dartMinions.Length * 5))
        {
            foreach (Transform transCoord in dartMinionTransforms)
            {
                Vector3 offset = new Vector3(transCoord.position.x + 5f, transCoord.position.y, transCoord.position.z);
                GenericActions.BulletAttack(bullet, team, character, Instantiate(bullet.bulletPrefab, offset, Quaternion.identity), Vector3.right);
                
            }
            if (GlobalSoundManager.IsWithinRange(gameObject)) GlobalSoundManager.GlobalSoundPlayer.PlayOneShot(dartRangedAttackClip, 0.7f);
            dartRangedAttackTime = Time.time;
        }
    }

    public override void PerformAttack()
    {
        if (animator.gameObject.activeSelf)
        {
            int random = UnityEngine.Random.Range(1, 3);
            switch (random)
            {
                case 1:
                    animator.SetBool("melee1", true);
                    break;
                case 2:
                    animator.SetBool("melee2", true);
                    break;
            }
        }

        foreach (Transform meleeAttackPoint in meleeAttackPoints)
        {
            GenericActions.MeleeAttack(meleeAttackPoint, team, character);
        }
        Invoke("ResetAnimation", character.swingTime / 10 < 1f ? 1f : character.swingTime / 10);
    }

    public override void PerformRanged()
    {
        if (animator.gameObject.activeSelf)
        {
            int random = UnityEngine.Random.Range(1, 3);
            switch (random)
            {
                case 1:
                    animator.SetBool("ranged1", true);
                    break;
                case 2:
                    animator.SetBool("ranged2", true);
                    break;
            }
        }

        GenericActions.MissileAttack(ringBoomerang, team, character, Instantiate(ringBoomerang.prefab, transform.position, Quaternion.identity), Vector3.right, gameObject);
        
        Invoke("ResetAnimation", character.fireRate / 10);
    }

    public override void PerformHeal()
    {
        // Define your healing logic here
        // For example, increase Health or remove status effects
        UnityEngine.Debug.Log(character.characterName + " performs a heal!");
    }

    public override void PerformUltimate()
    {
        EnemyProfiling[] enemies = FindObjectsOfType<EnemyProfiling>();
        CharacterProfiling[] chars = FindObjectsOfType<CharacterProfiling>();

        List<GameObject> targets = new List<GameObject>();

        foreach (EnemyProfiling enemy in enemies) {
            if (enemy.team != team)
            {
                targets.Add(enemy.gameObject);
            }
        }
        foreach (CharacterProfiling character in chars) { 
            if (character.team != team)
            {
                targets.Add(character.gameObject);
            }
        }

        if (animator.gameObject.activeSelf)
        {
            if (!animator.GetBool("Ultimate") && UltimateTimer >= UltimateMeter) {
                animator.SetBool("Ultimate", true);
                UltimateTimer = UltimateMeter;
            }
           
            if(UltimateTimer > 0)
            { 
                if (Time.time - homingRaysDelay >= character.fireRate / (dartMinions.Length * 20))
                {
                    float RandX = Random.Range(0f, 1.01f)
                        , RandY = Random.Range(0f, 1.01f);

                    int selectedTarget = Random.Range(0, targets.Count);
                    GameObject rays = Instantiate(homingRays.prefab, transform.position, Quaternion.identity);
                    GenericActions.MissileAttack(homingRays, team, character, rays, new Vector3(RandX, RandY, 0), gameObject);
                    rays.GetComponent<MissileController>().SetTarget(targets.ToArray()[selectedTarget]);
                    homingRaysDelay = Time.time;
                }
                       
            } else
            {
                animator.SetBool("Ultimate", false);
            }

        }
        UnityEngine.Debug.Log(character.characterName + " performs their ultimate ability!");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<MissileController>() != null)
        {
            MissileController retrievedBoomerang = collision.gameObject.GetComponent<MissileController>();
            if (retrievedBoomerang.target == gameObject.transform)
            {
                rangedAttackTime -= character.fireRate;
            }
        }
    }
}
