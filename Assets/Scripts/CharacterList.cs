using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CharacterList
{
    public static List<Character> characters = new List<Character>
    {
        new Character(LoadSpriteByGUID("0fe7b0717ff2e164a956115a709e873a"), "RI_241", "Mythri'theia", 1000f, 0f, 100f, 0f, 65.6f, 1.2f, 0f),
        new Character(LoadSpriteByGUID("0fe7b0717ff2e164a956115a709e873a"), "LI_K21", "Kiaj'uyiana", 1000f, 0f, 100f, 0f, 65.6f, 1.2f, 0f),
        new Character(LoadSpriteByGUID("0fe7b0717ff2e164a956115a709e873a"), "LI_S22", "Shiar'iakyia", 1000f, 0f, 100f, 0f, 65.6f, 1.2f, 0f),
        new Character(LoadSpriteByGUID("0fe7b0717ff2e164a956115a709e873a"), "VAL_K42", "Krist'ven", 1000f, 0f, 100f, 0f, 65.6f, 1.2f, 0f),
        new Character(LoadSpriteByGUID("0fe7b0717ff2e164a956115a709e873a"), "SP_SPAR32", "Spa'riat", 1000f, 0f, 100f, 0f, 65.6f, 1.2f, 0f),
    };


    private static Sprite LoadSpriteByGUID(string spriteGUID)
    {
        string path = UnityEditor.AssetDatabase.GUIDToAssetPath(spriteGUID);
        return UnityEditor.AssetDatabase.LoadAssetAtPath<Sprite>(path);
    }

    private static Animator LoadAnimatorByGUID(string spriteGUID)
    {
        string path = UnityEditor.AssetDatabase.GUIDToAssetPath(spriteGUID);
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

public class Character
{
    // character identification
    private string characterId;
    private string characterName;
    public Sprite characterSprite;
    public Animator animator;

    // stat controls
    private float maximumHealth;
    private float health;
    private float shootingRange;
    private float rangedDamage;
    private float meleeRange;
    private float meleeDamage;
    private float swingTime;
    private float fireRate;

    private InnateSkill innateSkill; //native character skill
    private ActiveSkill activeSkill; //additional skill, which any char can have
    private PassivePerk passivePerk; //each chars will have an unique passive perk

    public Character(Sprite sprite, string id, string name, float health, float shootingRange, float meleeRange, float rangedDamage, float meleeDamage, float swingTime, float fireRate)
    {
        characterSprite = sprite;
        characterId = id;
        characterName = name;
        maximumHealth = health;
        this.shootingRange = shootingRange;
        this.meleeRange = meleeRange;
        this.rangedDamage = rangedDamage;
        this.meleeDamage = meleeDamage; 
        this.swingTime = swingTime;
        this.fireRate = fireRate;

        // Initialize other properties as needed.
    }

    //public Sprite getCharacterSprite()
    //{
    //    return characterSprite;
    //}
}

public abstract class Skill
{
    public abstract void function();
}

public abstract class Passive
{
    public abstract void function();
}

public class PassivePerk : Passive
{
    override public void function()
    {

    }
}

public class CharacterPerk : Passive
{
    override public void function()
    {

    }
}

public class InnateSkill : Skill
{
    override public void function()
    {

    }
}

public class ActiveSkill : Skill
{
    override public void function()
    {

    }
}