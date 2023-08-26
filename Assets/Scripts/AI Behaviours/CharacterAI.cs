using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Unity.VisualScripting;
using UnityEngine;

public class CharacterAI : MonoBehaviour
{
    private CharacterProfiling characterProfiling;
    private MoveToMouse moveToMouse;
    public bool inMeleeRange { get; private set; }


    private void Awake()
    {
        inMeleeRange = false;
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

    
}
