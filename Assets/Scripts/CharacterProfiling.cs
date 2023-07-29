using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterProfiling : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    
    [SerializeField] private Character character;
    private int partyPositions;

    // status control
    private enum CharacterStatus { InMelee, Silenced, Stunned, Immobilized }
    private bool isDead = false;
    private int RespawnTimer = 0;
    private CharacterStatus[] currentStatuses = new CharacterStatus[0];

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (character.getCharacterSprite() != null) spriteRenderer.sprite = character.getCharacterSprite();
    }

    // Update is called once per frame
    void Update()
    {

    }
}



