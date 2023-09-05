using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class Passive : ScriptableObject
{
    public abstract void function();
}

[CreateAssetMenu(menuName = "Passive - Perk")]
public class PassivePerk : Passive
{
    override public void function()
    {

    }
}

[CreateAssetMenu(menuName = "Character - Perk")]
public class CharacterPerk : Passive
{
    override public void function()
    {

    }
}
