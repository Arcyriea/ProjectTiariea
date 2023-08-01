using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Enums
{
    public enum StatusEffectType { InMelee, Silenced, Stunned, Immobilized }
    public enum ShieldType { NONE, ARCANIC, TECHNICAL, HYBRID }
    public enum ArmorType { NONE, MYSTICAL, LIGHT, HEAVY, ENERGIZED, CHAOS, ANIMAL } //For modifier with def stat against certain damage counters or vulnerabilities to specific damage types
    public enum DamageType { PHYSICAL, EXPLOSIVE, ENERGY, MAGIC, CHAOS, POACHER, ANIMAL }
    public enum ElementType { HOLY, DARK, QUANTUM, IMAGINARY, WIND, FIRE, WATER, ICE, LIGHTNING, GENOMIC, VEGETATION, RADIOACTIVE }

    public enum ClassType { RANGER, MAGICIAN, SORCERER, DUALBLADE, GUNSLINGER, SCYTHE, GREATSWORD, CLERIC, GUARDIAN, SNIPER, LANCER, BEAMER, GRENADIER }

    public enum Team { ALLIES, HARMONICA, IMPERICA, DARK }
}
