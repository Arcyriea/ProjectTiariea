using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Character")]
public class Character : ScriptableObject
{
    // character identification
    public string characterId;
    public string characterName;
    public Sprite characterSprite;
    public Animator characterAnimator;
    public Enums.ClassType characterClass;

    // stat controls
    public float maximumHealth;
    public float maximumShield;

    public Enums.ShieldType ShieldIdentity;
    public float defense; //damage reduction stat
    public float maximumEnergy; //Used for Shield regeneration if either TECNICAL or HYBRID, can also be used to power up energy weaponries
    public float maximumMana; //For characters that is capable of arcanic capability, could act as alternate for shield generation if their type is ARCANIC or HYBRID
    public int maximumAmmo; //For ranged characters which uses physical ammunitions
    public float maximumGas; //For ranged char that uses liquid to fuel their gas based weaponries
    public float shootingRange;
    public float rangedDamage;
    public float fireRate;
    public float meleeRange;
    public float meleeDamage;
    public float swingTime;
    public float movementSpeed; //control the speed the character will relocate to a new position within the party formation


    [SerializeField] public InnateSkill innateSkill; //native character skill
    [SerializeField] public ActiveSkill activeSkill; //additional skill, which any char can have

    [SerializeField] public CharacterPerk characterPerk;

    [SerializeField] public PassivePerk passivePerk; //each chars will have an unique passive perk

    public Character(Sprite sprite, Animator animator, string id, string name, float health, float shield, float shootingRange, float meleeRange, float rangedDamage, float meleeDamage, float swingTime, float fireRate
        , CharacterPerk characterPerk, PassivePerk passivePerk, InnateSkill innateSkill, ActiveSkill activeSkill, float movementSpeed)
    {
        characterSprite = sprite;
        characterAnimator = animator;
        characterId = id;
        characterName = name;
        maximumHealth = health;
        maximumShield = shield;
        this.shootingRange = shootingRange;
        this.meleeRange = meleeRange;
        this.rangedDamage = rangedDamage;
        this.meleeDamage = meleeDamage;
        this.swingTime = swingTime;
        this.fireRate = fireRate;
        this.characterPerk = characterPerk;
        this.passivePerk = passivePerk;
        this.innateSkill = innateSkill;
        this.activeSkill = activeSkill;
        this.movementSpeed = movementSpeed;
        // Initialize other properties as needed.
    }
}

public static class CharacterList
{
    public static List<Character> characters = new List<Character>
    {
        //new Character(LoadSpriteByGUID("0fe7b0717ff2e164a956115a709e873a"), null, "RI_241", "Mythri'theia", 1000f, null, 0f, 100f, 0f, 65.6f, 1.2f, 0f, null, null, null, null),
        //new Character(LoadSpriteByGUID("0fe7b0717ff2e164a956115a709e873a"), null, "LI_K21", "Kiaj'uyiana", 1000f, 500f, 0f, 100f, 0f, 65.6f, 1.2f, 0f, null, null, null, null),
        //new Character(LoadSpriteByGUID("0fe7b0717ff2e164a956115a709e873a"), null, "LI_S22", "Shiar'iakyia", 1000f, 500f, 0f, 100f, 0f, 65.6f, 1.2f, 0f, null, null, null, null),
        //new Character(LoadSpriteByGUID("0fe7b0717ff2e164a956115a709e873a"), null, "VAL_K42", "Krist'ven", 1000f, null, 0f, 100f, 0f, 65.6f, 1.2f, 0f, null, null, null, null),
        //new Character(LoadSpriteByGUID("0fe7b0717ff2e164a956115a709e873a"), null, "SP_SPAR32", "Spa'riat", 1000f, null, 0f, 100f, 0f, 65.6f, 1.2f, 0f, null, null, null, null),
    };


    private static Sprite LoadSpriteByGUID(string spriteGUID)
    {
        string path = UnityEditor.AssetDatabase.GUIDToAssetPath(spriteGUID);
        return UnityEditor.AssetDatabase.LoadAssetAtPath<Sprite>(path);
    }

    private static Animator LoadAnimatorByGUID(string animGUID)
    {
        string path = UnityEditor.AssetDatabase.GUIDToAssetPath(animGUID);
        return UnityEditor.AssetDatabase.LoadAssetAtPath<Animator>(path);
    }
}

public static class InnateSkills
{
    public static List<InnateSkill> innateSkills = new List<InnateSkill>
    {

    };
}

public static class ActiveSkills
{
    public static List<ActiveSkill> innateSkills = new List<ActiveSkill>
    {

    };
}

public static class PassivePerks
{
    public static List<PassivePerk> innateSkills = new List<PassivePerk>
    {

    };
}







