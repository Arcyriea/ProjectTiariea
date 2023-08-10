using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterProfiling : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;

    public Character character { get; private set; }
    private int partyPositions;

    public float Health { get; private set; }
    public float? Shield { get; private set; }
    public float? Mana { get; private set; }
    public int Lives { get; private set; }

    protected MoveToMouse moveToMouse { get; private set; }

    // status control
    public bool isDead { get; private set; }
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

    public CharacterProfiling()
    {

    }

    void Awake()
    {
        isDead = false;
    }

    // Start is called before the first frame update
    protected virtual void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        moveToMouse = GetComponent<MoveToMouse>();
        if (character == null) return;
        Health = character.maximumHealth;
        Shield = character.maximumShield;
        Mana = character.maximumMana;
        Lives = 3 + character.LivesModifier;
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        if (Health <= 0)
        {
            isDead = true;
            RespawnTimer = 5;
        }
        if (Lives > 0) CheckRespawn();
    }

    private void CheckRespawn()
    {
        if (isDead)
        {
            character.characterPrefab.SetActive(false);
            RespawnTimer -= (int) Time.fixedDeltaTime;
            if (RespawnTimer <= 0)
            {
                character.characterPrefab.SetActive(true);
                ResetStats();
                RespawnTimer = 0;
                isDead = false;
                Lives -= 1;
            }
        }
    }

    void ResetStats()
    {
        Health = character.maximumHealth;
        Shield = character.maximumShield;
        Mana = character.maximumMana;
    }

    public void Heal(float Healing)
    {
        Health += Healing;
        if (Health > character.maximumHealth) Health = character.maximumHealth;
    }

    public void grantExtraLives(int Lives)
    {
        this.Lives += Lives;
    }

    public void GetCharacterFromScriptableObject(Character character)
    {
        this.character = character;
    }

    public virtual void CharacterAction(string action)
    {

    }

}



