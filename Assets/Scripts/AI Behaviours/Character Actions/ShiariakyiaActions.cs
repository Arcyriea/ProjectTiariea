using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class ShiariakyiaActions : CharacterProfiling
{
    public BulletProperties bullet;
    public Transform[] meleeAttackPoints;

    public GameObject[] dartMinions;
    private Transform[] dartMinionTransforms;
    private float orbitRadius = 4f;
    private float dartRangedAttackTime;
    public override void CharacterAction(string action)
    {
        GenericActions.ExecuteAction(this, action);
    }

    protected override void Start()
    {
        base.Start();
        InitializeDarts();
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        DartOrbit();
        if (base.moveToMouse.selected == true)
        {
            if (Input.GetKeyDown(KeyCode.Space))
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
        CharacterAction("ranged");
    }

    public override void SyncMeleeAttacks()
    {
        CharacterAction("attack");
    }

    private void InitializeDarts()
    {
        dartMinionTransforms = new Transform[dartMinions.Length];
        for (int i = 0; i < dartMinions.Length; i++)
        {
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
        float orbitSpeed = 30f; // Adjust the speed of orbit

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
        if (Time.time - dartRangedAttackTime >= character.fireRate / (dartMinions.Length * 3))
        {
            foreach (Transform transCoord in dartMinionTransforms)
            {
                Vector3 offset = new Vector3(transCoord.position.x + 5f, transCoord.position.y, transCoord.position.z);
                GenericActions.BulletAttack(bullet, team, character, Instantiate(bullet.bulletPrefab, offset, Quaternion.identity), Vector3.right);
            }

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
        Invoke("ResetAnimation", character.swingTime / 10);

        UnityEngine.Debug.Log(character.characterName + " performs an attack!");
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

        
        
        Invoke("ResetAnimation", character.fireRate / 10);

        UnityEngine.Debug.Log(character.characterName + " performs ranged attack!");
    }

    public override void PerformHeal()
    {
        // Define your healing logic here
        // For example, increase Health or remove status effects
        UnityEngine.Debug.Log(character.characterName + " performs a heal!");
    }

    public override void PerformUltimate()
    {
        // Define your ultimate ability logic here
        // For example, deal massive damage or apply powerful effects
        UnityEngine.Debug.Log(character.characterName + " performs their ultimate ability!");
    }
}
