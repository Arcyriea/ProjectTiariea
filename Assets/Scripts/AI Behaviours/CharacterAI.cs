using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class CharacterAI : MonoBehaviour
{
    private CharacterProfiling characterProfiling;
    private MoveToMouse moveToMouse;
    private bool inMeleeRange = false;

    private void Start()
    {
        characterProfiling = GetComponent<CharacterProfiling>();
        moveToMouse = GetComponent<MoveToMouse>();
    }

    private void Update()
    {
        

        if (!moveToMouse.selected) // Check if the character is unselected
        {
            if (inMeleeRange)
            {
                if (characterProfiling.meleeAttackTime >= characterProfiling.character.swingTime) PerformAttack("melee");
            }
            else if (!inMeleeRange)
            {
                if (characterProfiling.rangedAttackTime >= characterProfiling.character.fireRate) PerformAttack("ranged");
            }
        }
    }

    protected virtual void PerformAction(string actionType)
    {

    }

    protected void PerformAttack(string attackType)
    {

        // Detect enemies within melee range
        Collider2D[] hitColliders;
        Collider2D[] meleeCheck = Physics2D.OverlapCircleAll(transform.position, characterProfiling.character.meleeRange);
        List<Collider2D> enemies = new List<Collider2D>();

        foreach (Collider2D meleeCollider in meleeCheck)
        {
            if (meleeCollider.CompareTag("Enemy"))
            {
                enemies.Add(meleeCollider);
                inMeleeRange = true;
            }
        }

        if (enemies.Count <= 0 && inMeleeRange != false)
        {
            inMeleeRange = false;
            return;
        }


        if (attackType == "melee")
        {
            hitColliders = meleeCheck;
        }
        else
        {
            hitColliders = Physics2D.OverlapCircleAll(transform.position, characterProfiling.character.shootingRange);
        }


        foreach (Collider2D hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("Enemy"))
            {
                switch (attackType)
                {
                    case "melee":
                        characterProfiling.CharacterAction("attack");
                        //UnityEngine.Debug.Log(characterProfiling.character.characterName + " Performing melee attack on enemy: " + hitCollider.gameObject.name);
                        break;
                    case "ranged":
                        characterProfiling.CharacterAction("ranged");
                        //UnityEngine.Debug.Log(characterProfiling.character.characterName + " Performing ranged attack on enemy: " + hitCollider.gameObject.name);
                        break;
                }
            }
        }


    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, characterProfiling.character.meleeRange);

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, characterProfiling.character.shootingRange);
    }
}
