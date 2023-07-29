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
    private LinkedList<CharacterStatus> currentStatuses = new LinkedList<CharacterStatus>();

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (character.characterSprite != null) spriteRenderer.sprite = character.characterSprite;
    }

    // Update is called once per frame
    void Update()
    {

    }
}



