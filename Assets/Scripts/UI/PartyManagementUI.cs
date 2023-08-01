using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartyManagementUI : MonoBehaviour
{
    public PartyController partyController; // Reference to the PartyController script

    // Function to add a character to the party
    public void AddCharacterToParty(Character character)
    {
        // Add the character to the party controller's characters array
        partyController.AddCharacterToParty(character);
    }

    // Function to remove a character from the party
    public void RemoveCharacterFromParty(Character character)
    {
        // Remove the character from the party controller's characters array
        partyController.RemoveCharacterFromParty(character);
    }

    // Function to rearrange the formation of the party
    public void RearrangeFormation(CustomFormation formation)
    {
        // Set the selected formation in the party controller
        partyController.SetSelectedFormation(formation);
    }

    // Other functions related to party management can be added here
}
