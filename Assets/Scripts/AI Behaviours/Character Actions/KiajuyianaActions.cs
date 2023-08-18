using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class KiajuyianaActions : CharacterProfiling, IDefaultActions
{
    public BulletProperties bullet;
    public Transform meleeAttackPoint;
    

    public override void CharacterAction(string action)
    {
        switch (action)
        {
            case "attack":
                if (meleeAttackTime >= character.swingTime)
                {
                    PerformAttack();
                    meleeAttackTime = 0f;
                }
                break;
            case "heal":
                if (rangedAttackTime >= character.fireRate)
                {
                    PerformHeal();
                    rangedAttackTime = 0f;
                }
                break;
            case "ranged":
                if (rangedAttackTime >= character.fireRate)
                {
                    PerformRanged();
                    rangedAttackTime = 0f;
                }
                break;
            case "ultimate":
                PerformUltimate();
                break;
            default:
                UnityEngine.Debug.LogError("Invalid action: " + action);
                break;
        }
    }

    protected override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        if (base.moveToMouse.selected == true)
        {
            if (Input.GetMouseButtonDown(0) || Input.GetKey(KeyCode.Space))
            {
                CharacterAction("ranged");
            }

            if (Input.GetMouseButtonDown(1))
            {
                CharacterAction("attack");
            }
        }
    }

    public void PerformAttack()
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
        GenericActions.MeleeAttack(meleeAttackPoint, team, character);
        Invoke("ResetAnimation", 0.15f);

        UnityEngine.Debug.Log(character.characterName + " performs an attack!");
    }

    public void PerformRanged()
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
        GenericActions.BulletAttack(bullet, team, character, Instantiate(bullet.bulletPrefab, transform.position, Quaternion.identity), Vector3.right);
        Invoke("ResetAnimation", 0.15f);

        UnityEngine.Debug.Log(character.characterName + " performs ranged attack!");
    }

    public void PerformHeal()
    {
        // Define your healing logic here
        // For example, increase Health or remove status effects
        UnityEngine.Debug.Log(character.characterName + " performs a heal!");
    }

    public void PerformUltimate()
    {
        // Define your ultimate ability logic here
        // For example, deal massive damage or apply powerful effects
        UnityEngine.Debug.Log(character.characterName + " performs their ultimate ability!");
    }

    void OnDrawGizmosSelected()
    {
        if (meleeAttackPoint == null) return;
        Gizmos.DrawWireSphere(meleeAttackPoint.position, 1.5f);
    }

    private void ResetAnimation()
    {
        if (animator.GetBool("melee1") != false) animator.SetBool("melee1", false);
        if (animator.GetBool("melee2") != false) animator.SetBool("melee2", false);
        if (animator.GetBool("ranged1") != false) animator.SetBool("ranged1", false);
        if (animator.GetBool("ranged2") != false) animator.SetBool("ranged2", false);
    }
}
