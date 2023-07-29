using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CharacterList
{
    public static List<Character> characters = new List<Character>
    {
        new Character(null, "RI_241", "Mythri'theia", 1000f, 0f, 100f, 0f, 65.6f, 1.2f, 0f),
        new Character(null, "LI_K21", "Kiaj'uyiana", 1000f, 0f, 100f, 0f, 65.6f, 1.2f, 0f),
        new Character(null, "LI_S22", "Shiar'iakyia", 1000f, 0f, 100f, 0f, 65.6f, 1.2f, 0f),
        new Character(null, "VAL_K42", "Krist'ven", 1000f, 0f, 100f, 0f, 65.6f, 1.2f, 0f),
        new Character(null, "SP_SPAR32", "Spa'riat", 1000f, 0f, 100f, 0f, 65.6f, 1.2f, 0f),
    };
    
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
    private Sprite characterSprite;

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

    public Sprite getCharacterSprite()
    {
        return characterSprite;
    }
}

public abstract class Skill
{

}

public class PassivePerk
{

}

public class InnateSkill : Skill
{

}

public class ActiveSkill : Skill
{

}