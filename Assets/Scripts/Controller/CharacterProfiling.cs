using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterProfiling : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    
    public Character character;
    private Animator animator;
    private int partyPositions;

    public float Health { get; private set; }
    public float? Shield { get; private set; }
    public float? Mana { get; private set; }

    // status control
    private bool isDead = false;
    private int RespawnTimer = 0;

    private class StatusEffect
    {
        public Enums.StatusEffectType type;
        public int stackCount;
        public int maxStack;
        public float duration;

        public StatusEffect(Enums.StatusEffectType type, int stackCount, float duration)
        {
            this.type = type;
            this.stackCount = stackCount;
            this.duration = duration;
        }
    }

    private LinkedList<StatusEffect> statusEffects = new LinkedList<StatusEffect>();
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
        Health = character.maximumHealth;
        Shield = character.maximumShield;
        Mana = character.maximumMana;
        if (character.characterSprite != null) spriteRenderer.sprite = character.characterSprite;
        if (character.characterAnimator != null) animator = character.characterAnimator;
    }

    // Update is called once per frame
    void Update()
    {

    }
}



