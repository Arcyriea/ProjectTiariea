using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Security.Cryptography;
using UnityEngine;

public class PartyController : MonoBehaviour
{
    // Start is called before the first frame update
    public float followSpeed = 2f;
    public float partyTravelSpeed = 60f;
    public Character[] characters;
    private float zoom;
    private int memCap = 6;
    public List<CustomFormation> customFormations;

    private List<GameObject> spawnedPrefabs = new List<GameObject>();
    private CustomFormation selectedFormation;
    private int selectedPrefabIndex = -1;
    public float spacing = 2f;

    public LayerMask prefabLayer;

    public float zoomSpeed = 5f; // Adjust this value to control the zoom speed
    public float minFOV = 30f; // Set the minimum FOV for zooming in
    public float maxFOV = 120f; // Set the maximum FOV for zooming out

    private void Start()
    {
        
        Vector3 spawnPosition = Camera.main.transform.position + new Vector3(0f, 0f, 10f);
        for (int i = 0; i < characters.Length; i++)
        {
            Vector3 offset = new Vector3(
                (i - (memCap - 1) / 2f) * spacing, //(i - (memCap - 1) / 2f) * spacing, 
                0f,
                0f);
            //CharacterProfiling prefabCharacter = partyMembers.GetComponent<CharacterProfiling>();
            //prefabCharacter.character = selectedCharacters[i];
            GameObject prefabInstance = Instantiate(characters[i].characterPrefab, spawnPosition + offset, Quaternion.identity);
            prefabInstance.layer = prefabLayer;
            CharacterProfiling prefabCharProfile = prefabInstance.GetComponent<CharacterProfiling>();

            prefabCharProfile.character = characters[i];

            UnityEngine.Debug.Log("Their name is: " + prefabCharProfile.character.characterName);
            spawnedPrefabs.Add(prefabInstance);

        }//Khoi tao nhan vat trong doi

        if (customFormations.Count > 0)
        {
            selectedFormation = customFormations[0];
            UpdateFormation();
        }//Sap xep theo doi hinh
    }

    void Update()
    {

        Vector3 newPos = new Vector3(Camera.main.transform.position.x + (partyTravelSpeed * Time.deltaTime), Camera.main.transform.position.y, -10f);
        Camera.main.transform.position = Vector3.Lerp(transform.position, newPos, followSpeed * Time.deltaTime);

        ZoomControl();
        PartyMarchCommand();

        foreach (GameObject member in spawnedPrefabs)
        {
            Vector3 memMove = new Vector3(member.transform.position.x + (partyTravelSpeed * Time.deltaTime), member.transform.position.y, 0f);
            //Vector3 offset = member.transform.position - transform.position;
            member.transform.position = Vector3.Lerp(member.transform.position, memMove, followSpeed * Time.deltaTime);// + offset;
        }

        Vector3 targetPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        targetPosition.z = 0f;

        // Handle input to move the selected prefab
        if (selectedPrefabIndex != -1 && Input.GetMouseButton(0))
        {
            spawnedPrefabs[selectedPrefabIndex].transform.position = targetPosition;
        }
    }

    private void UpdateFormation()
    {
        if (selectedFormation != null)
        {
            for (int i = 0; i < memCap && i < selectedFormation.prefabPositions.Length; i++)
            {
                spawnedPrefabs[i].transform.position = Camera.main.transform.position + selectedFormation.prefabPositions[i];
            }
        }
    }

    private void ZoomControl()
    {
        float zoomInput = Input.GetAxis("Mouse ScrollWheel");
        float newFOV = Camera.main.fieldOfView - zoomInput * zoomSpeed;
        newFOV = Mathf.Clamp(newFOV, minFOV, maxFOV); // Clamp the FOV within the specified range
        Camera.main.fieldOfView = newFOV;
    }

    private void PartyMarchCommand()
    {
        switch (Input.GetAxisRaw("Horizontal"))
        {
            case > 0:
                partyTravelSpeed = Mathf.Clamp(partyTravelSpeed + (Input.GetAxisRaw("Horizontal") * 0.6f), 0, 180f);
                break;
            case < 0:
                partyTravelSpeed = Mathf.Clamp(partyTravelSpeed + (Input.GetAxisRaw("Horizontal") * 0.6f), 0, 180f);
                break;
        }

        if (Input.GetKey(KeyCode.Space))
        {
            partyTravelSpeed = 0;
        }
    }

    public void AddCharacterToParty(Character character)
    {
        if (IsCharacterInParty(character))
        {
            UnityEngine.Debug.LogWarning("Character " + character.characterName + " is already in the party.");
            return;
        }

        if (characters.Length >= memCap)
        {
            UnityEngine.Debug.LogWarning("Party is already full. Cannot add more characters.");
            return;
        }

        Character[] updatedCharacters = new Character[characters.Length + 1];

        // Copy the existing characters to the new array
        for (int i = 0; i < characters.Length; i++)
        {
            updatedCharacters[i] = characters[i];
        }

        updatedCharacters[characters.Length] = character;
        characters = updatedCharacters;
    }

    public void RemoveCharacterFromParty(Character character)
    {
        if (!IsCharacterInParty(character))
        {
            UnityEngine.Debug.LogWarning("Character " + character.characterName + " is not in the party.");
            return;
        }

        Character[] updatedCharacters = new Character[characters.Length - 1];

        int newIndex = 0;
        for (int i = 0; i < characters.Length; i++)
        {
            if (characters[i] != character)
            {
                updatedCharacters[newIndex] = characters[i];
                newIndex++;
            }
        }

        characters = updatedCharacters;

    }

    // Function to check if a character is in the party
    public bool IsCharacterInParty(Character character)
    {
        foreach (Character partyCharacter in characters)
        {
            if (partyCharacter == character)
            {
                return true;
            }
        }
        return false;
    }

    public void SetSelectedFormation(CustomFormation formation)
    {

    }
}
