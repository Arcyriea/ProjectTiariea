using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class CharacterAI : MonoBehaviour
{
    private CharacterProfiling characterProfiling;
    private MoveToMouse moveToMouse;
    public bool inMeleeRange { get; private set; }
    public bool enemyInRange { get; private set; }

    private void Awake()
    {
        inMeleeRange = false;
        enemyInRange = false;
    }
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
                characterProfiling.SyncMeleeAttacks();
            }
            else if (!inMeleeRange)
            {
                characterProfiling.SyncRangedAttacks();
            }
        }
    }

    public void ReportStatus(bool status)
    {
        inMeleeRange = status;

        if (inMeleeRange && enemyInRange == false) { enemyInRange = true; }
    }

    public bool RangedDetection(Transform shootingPoint)
    {
        RaycastHit2D[] rangedCheck = Physics2D.RaycastAll(shootingPoint.position, transform.right * (characterProfiling.character.shootingRange / 2));
        if (rangedCheck.Length > 0) {
            foreach(RaycastHit2D hit in rangedCheck)
            {
                CharacterProfiling chara = hit.collider.gameObject.GetComponent<CharacterProfiling>();
                EnemyProfiling enemy = hit.collider.gameObject.GetComponent<EnemyProfiling>();
                if (chara != null)
                {
                    if (chara.team != characterProfiling.team) return true;
                }
                if (enemy != null)
                {
                    if (enemy.team != characterProfiling.team) return true;
                }
            }
        }
        return false;
    }
    public bool MeleeDetection(Transform meleeAttackPoint)
    {
        
        Collider2D[] meleeCheck = Physics2D.OverlapCircleAll(meleeAttackPoint.position, characterProfiling.character.meleeRange);

        foreach (Collider2D meleeCollider in meleeCheck)
        {
            EnemyProfiling entity = meleeCollider.GetComponent<EnemyProfiling>();
            CharacterProfiling chara = meleeCollider.GetComponent<CharacterProfiling>();
            if (entity != null && entity.team != characterProfiling.team)
            {
                return true;
            }
            if (chara != null && chara.team != characterProfiling.team)
            {
                return true;
            }
        }
        return false;
    }

    private void OnDrawGizmosSelected()
    {

        Physics2D.RaycastAll(transform.position, transform.right * (characterProfiling.character.shootingRange / 2));
        Gizmos.DrawLine(transform.position, transform.right * (characterProfiling.character.shootingRange / 2));
    }
}
