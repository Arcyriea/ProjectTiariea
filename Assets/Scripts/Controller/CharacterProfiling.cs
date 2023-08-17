using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterProfiling : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    protected Animator animator;

    public Character character { get; private set; }
    private int partyPositions;

    public float Health { get; private set; }
    public float Shield { get; private set; }
    public float Mana { get; private set; }
    public float Energy { get; private set; }
    public int Lives { get; private set; }
    public Enums.Team team { get; private set; }

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

    // Start is called before the first frame update
    protected virtual void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        moveToMouse = GetComponent<MoveToMouse>();
        if (character == null) return;
        animator = character.characterPrefab.GetComponent<Animator>();
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

    void ResetStats()
    {
        Health = character.maximumHealth;
        Shield = character.maximumShield;
        Mana = character.maximumMana;
        Energy = character.maximumEnergy;
    }

    public void TakeDamage(float Damage)
    {
        Health -= Damage;
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

    public void SetTeam(Enums.Team team)
    {
        this.team = team;
    }

    //protected LayerMask GenerateEnemyLayerMask(Enums.Team ownTeam)
    //{
    //    int allEnemyLayers = GetAllEnemyLayers(ownTeam);
    //    return (1 << allEnemyLayers);
    //}

    //protected int GetAllEnemyLayers(Enums.Team ownTeam)
    //{
    //    int allEnemyLayers = 0;
    //    foreach (Enums.Team team in System.Enum.GetValues(typeof(Enums.Team)))
    //    {
    //        if (team != ownTeam)
    //        {
    //            int teamLayer = LayerMask.NameToLayer(team.ToString());
    //            if (teamLayer != -1) // Check if the layer exists
    //            {
    //                allEnemyLayers |= (1 << teamLayer);
    //            }
    //        }
    //    }
    //    return allEnemyLayers;
    //}

}



