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
        GenericActions.ExecuteAction(this, action);
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

    public override void PerformAttack()
    {
        if (animator.gameObject.activeSelf) {


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
        
        GenericActions.MeleeAttack(meleeAttackPoint, team, character);
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

        GenericActions.BulletAttack(bullet, team, character, Instantiate(bullet.bulletPrefab, transform.position, Quaternion.identity), Vector3.right);
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

    void OnDrawGizmosSelected()
    {
        if (meleeAttackPoint == null) return;
        Gizmos.DrawWireSphere(meleeAttackPoint.position, 1.5f);
    }

    private void ResetAnimation()
    {
        string[] animationNames = { "melee1", "melee2", "ranged1", "ranged2" };

        foreach(string animationName in animationNames)
        {
            if (animator.GetBool(animationName) != false) animator.SetBool(animationName, false);
        }
    }
}
