using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class MythritheiaActions : CharacterProfiling
{
    public override void CharacterAction(string action)
    {
        switch (action)
        {
            case "attack":
                PerformAttack();
                break;
            case "heal":
                PerformHeal();
                break;
            case "ranged":
                PerformRanged();
                break;
            case "ultimate":
                PerformUltimate();
                break;
            default:
                UnityEngine.Debug.LogError("Invalid action: " + action);
                break;
        }
    }

    protected override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        if (base.moveToMouse.selected == true)
        {

        }
    }

    private void PerformAttack()
    {
        // Define your attack logic here
        // For example, reduce enemy health or apply status effects
        UnityEngine.Debug.Log(character.characterName + " performs an attack!");
    }

    private void PerformRanged()
    {
        // Define your attack logic here
        // For example, reduce enemy health or apply status effects
        UnityEngine.Debug.Log(character.characterName + " performs ranged attack!");
    }

    private void PerformHeal()
    {
        // Define your healing logic here
        // For example, increase Health or remove status effects
        UnityEngine.Debug.Log(character.characterName + " performs a heal!");
    }

    private void PerformUltimate()
    {
        // Define your ultimate ability logic here
        // For example, deal massive damage or apply powerful effects
        UnityEngine.Debug.Log(character.characterName + " performs their ultimate ability!");
    }
}
