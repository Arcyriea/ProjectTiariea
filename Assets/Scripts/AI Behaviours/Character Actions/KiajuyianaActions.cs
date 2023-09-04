using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Unity.VisualScripting;
using UnityEngine;

public class KiajuyianaActions : CharacterProfiling, IDefaultActions
{
    public BulletProperties bullet;
    public Transform meleeAttackPoint;

    public AudioClip swordWhoosh;
    public AudioClip[] swordSwing;

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
            if (Input.GetKey(KeyCode.Space))
            {
                CharacterAction("ranged");
            }

            if (Input.GetMouseButton(1))
            {
                CharacterAction("attack");
            }
        }
        else
        {
            ControlAI();
        }
    }

    protected override void ControlAI()
    {
        if (commandAI.MeleeDetection(meleeAttackPoint))
        {
            commandAI.ReportStatus(true);
        } else
        {
            commandAI.ReportStatus(false);
        }
    }

    public override void SyncMeleeAttacks()
    {
        CharacterAction("attack");
    }

    public override void SyncRangedAttacks()
    {
        CharacterAction("ranged");
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
        int sound = UnityEngine.Random.Range(1, swordSwing.Length);
        if (GlobalSoundManager.IsWithinRange(gameObject)) GlobalSoundManager.GlobalSoundPlayer.PlayOneShot(swordSwing[sound], 1f);
        Invoke("ResetAnimation", character.swingTime / 10 < 1f ? 1f : character.swingTime / 10);

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
        if (GlobalSoundManager.IsWithinRange(gameObject)) GlobalSoundManager.GlobalSoundPlayer.PlayOneShot(swordWhoosh, 1f);
        Invoke("ResetAnimation", character.fireRate / 10);

        //UnityEngine.Debug.Log(character.characterName + " performs ranged attack!");
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
       
    }
}
