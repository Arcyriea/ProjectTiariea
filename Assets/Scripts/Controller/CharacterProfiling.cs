using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterProfiling : MonoBehaviour, IDefaultActions
{
    private SpriteRenderer spriteRenderer;
    protected Animator animator;
    protected CharacterAI commandAI;
    public Character character { get; private set; }
    private int partyPositions;

    public float Health { get; private set; }
    public float Shield { get; private set; }
    public float Mana { get; private set; }
    public float Energy { get; private set; }
    public int Lives { get; private set; }
    public Enums.Team team { get; private set; }

    public float UltimateMeter { get; private set; }

    public float meleeAttackTime { get; set; }
    public float rangedAttackTime { get; set; }

    protected MoveToMouse moveToMouse { get; private set; }
    //protected LayerMask enemyLayerMask;

    // status control
    public bool isDead { get; private set; }
    private int RespawnTimer = 0;
    public bool punctured = false;
    private int punctureCooldown;



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

    public virtual void SyncRangedAttacks()
    {
        throw new NotImplementedException();
    }

    public virtual void SyncMeleeAttacks()
    {
        throw new NotImplementedException();
    }
    // Start is called before the first frame update
    protected virtual void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        moveToMouse = GetComponent<MoveToMouse>();
        commandAI = GetComponent<CharacterAI>();
        if (character == null) return;
        animator = GetComponent<Animator>();
        if (animator == null) UnityEngine.Debug.LogWarning("Animator failed to be retrieved for the CharacterProfiling");

        Health = character.maximumHealth;
        Shield = character.maximumShield;
        Mana = character.maximumMana;
        Energy = character.maximumEnergy;
        Lives = 3 + character.LivesModifier;
        //GenerateEnemyLayerMask(team);

    }

    // Update is called once per frame
    protected virtual void Update()
    {
        Regeneration();

        if (punctured && punctureCooldown <= 0)
        {
            punctureCooldown = 3;
        }

        punctureCooldown -= (int)Time.fixedDeltaTime;
        if (punctureCooldown <= 0) punctured = false;


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
            RespawnTimer -= (int)Time.fixedDeltaTime;
            Lives -= 1;
            if (RespawnTimer <= 0)
            {
                character.characterPrefab.SetActive(true);
                ResetStats();
                RespawnTimer = 0;
                isDead = false;

            }
        }
    }

    protected virtual void Regeneration()
    {
        if (Health < character.maximumHealth && character.healthRegen != 0) Health += character.healthRegen;
        if (Shield < character.maximumShield && character.shieldRegen != 0) Shield += character.shieldRegen;
        if (Energy < character.maximumEnergy && character.energyRegen != 0) Energy += character.energyRegen;
        if (Mana < character.maximumMana && character.manaRegen != 0) Mana += character.manaRegen;

    }

    void ResetStats()
    {
        Health = character.maximumHealth;
        Shield = character.maximumShield;
        Mana = character.maximumMana;
        Energy = character.maximumEnergy;
    }

    public void TakeDamage(float Damage)
    {
        if (Shield > 0) Shield -= Damage;
        else Health -= Damage;
    }

    public void TakeHealing(float Healing, bool healShield)
    {
        Health += Healing;
        if (Health > character.maximumHealth)
        {
            if (character.maximumShield > 0 && healShield)
            {
                float excessHealing = (Health - character.maximumHealth);
                Shield += excessHealing;
                if (Shield > character.maximumShield) Shield = character.maximumShield;
            }
            Health = character.maximumHealth;
        }
    }

    public void RestoreShield(float Healing)
    {
        Shield += Healing;
        if (Shield > character.maximumShield) Shield = character.maximumShield;
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
        throw new NotImplementedException();
    }

    public void SetTeam(Enums.Team team)
    {
        this.team = team;
    }

    public virtual void PerformAttack()
    {
        throw new NotImplementedException();
    }

    public virtual void PerformRanged()
    {
        throw new NotImplementedException();
    }

    public virtual void PerformHeal()
    {
        throw new NotImplementedException();
    }

    public virtual void PerformUltimate()
    {
        throw new NotImplementedException();
    }

    protected void ResetAnimation()
    {
        string[] animationNames = { "melee1", "melee2", "ranged1", "ranged2" };

        foreach (string animationName in animationNames)
        {
            if (animator.GetBool(animationName) != false) animator.SetBool(animationName, false);
        }
    }

    protected virtual void ControlAI()
    {
        throw new NotImplementedException();
    }
}



