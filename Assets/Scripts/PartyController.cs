using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Security.Cryptography;
using UnityEngine;

public class PartyController : MonoBehaviour
{
    // Start is called before the first frame update
    public float followSpeed = 2f;
    public float partyTravelSpeed = 60f;
    public GameObject partyMembers;
    private float zoom;
    public int memCap = 6;

    public List<Character> selectedCharacters = new List<Character> ();
    public List<CustomFormation> customFormations;

    private List<GameObject> spawnedPrefabs = new List<GameObject>();
    private CustomFormation selectedFormation;
    private int selectedPrefabIndex = -1;
    public float spacing = 2f;

    private void Start()
    {
        Vector3 spawnPosition = Camera.main.transform.position;
        for (int i = 0; i < memCap && i < selectedCharacters.Count - 1; i++)
        {
            Vector3 offset = new Vector3((i - (memCap - 1) / 2f) * spacing, 0f, 0f);
            CharacterProfiling prefabCharacter = partyMembers.GetComponent<CharacterProfiling>();
            prefabCharacter.character = selectedCharacters[i];
            GameObject prefabInstance = Instantiate(partyMembers, spawnPosition + offset, Quaternion.identity);
            
            if(prefabCharacter != null) spawnedPrefabs.Add(prefabInstance);
            else Destroy(prefabInstance.gameObject);
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

        foreach (GameObject member in spawnedPrefabs)
        {
            Vector3 memMove = new Vector3(member.transform.position.x + (partyTravelSpeed * Time.deltaTime), member.transform.position.y, -10f);
            Vector3 offset = member.transform.position - transform.position;
            member.transform.position = Vector3.Lerp(member.transform.position, memMove, followSpeed * Time.deltaTime) + offset;
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
}
