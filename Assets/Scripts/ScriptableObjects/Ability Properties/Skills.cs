using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class Skill : ScriptableObject
{
    public abstract void function();
}

[CreateAssetMenu(menuName = "Skill - Innate Character")]
public class InnateSkill : Skill
{
    override public void function()
    {

    }
}

[CreateAssetMenu(menuName = "Skill - Active Ability")]
public class ActiveSkill : Skill
{
    override public void function()
    {

    }
}
