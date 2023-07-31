using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterProfiling : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    
    [SerializeField] private Character character;
    private Animator animator;
    private int partyPositions;

    // status control
    private enum CharacterStatus { InMelee, Silenced, Stunned, Immobilized }
    private bool isDead = false;
    private int RespawnTimer = 0;
    private LinkedList<CharacterStatus> currentStatuses = new LinkedList<CharacterStatus>();
    public CharacterProfiling(Character character, int partyPositions)
    {
        this.character = character;
        this.partyPositions = partyPositions;
    }


    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (character == null) return;
        if (character.characterSprite != null) spriteRenderer.sprite = character.characterSprite;
        if (character.characterAnimator != null) animator = character.characterAnimator;
    }

    // Update is called once per frame
    void Update()
    {

    }
}



