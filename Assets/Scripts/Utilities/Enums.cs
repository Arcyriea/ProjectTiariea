using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Enums
{
    public enum StatusEffectType { InMelee, Silenced, Stunned, Immobilized }
    public enum ShieldType { ARCANIC, TECHNICAL, HYBRID, NONE }
    public enum ArmorType { MYSTICAL, LIGHT, HEAVY, CHAOS, ANIMAL, NONE } //For modifier with def stat against certain damage counters
    public enum DamageType { PHYSICAL, EXPLOSIVE, MAGIC, CHAOS }
    public enum ElementType { HOLY, DARK, QUANTUM, IMAGINARY, WIND, FIRE, WATER, ICE, LIGHTNING, GENOMIC, VEGETATION, RADIOACTIVE }

    public enum ClassType { RANGER, MAGICIAN, SORCERER, DUALBLADE, GUNSLINGER, SCYTHE, GREATSWORD, CLERIC, GUARDIAN, SNIPER, LANCER, BEAMER, GRENADIER }

    public enum Team { ALLIES, HARMONICA, IMPERICA, DARK }
}
