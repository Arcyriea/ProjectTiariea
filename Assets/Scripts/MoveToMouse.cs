using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Security.Cryptography;
using System.Threading;
using UnityEngine;

public class MoveToMouse : MonoBehaviour
{
    public static List<MoveToMouse> movementObjects = new List<MoveToMouse>();
    public float speed { get; private set; }
    public bool selected { get; private set; }

    private bool isInsideSelectionBox = false;
    private GameObject selectionBox;
    private SelectionBox selectScript;
    private Animator animator;


    void Awake()
    {
        selectionBox = GameObject.Find("BoxSelector");
        selectScript = selectionBox.GetComponent<SelectionBox>();
        CharacterProfiling characterProfiling = this.GetComponent<CharacterProfiling>();
        if (characterProfiling != null)
        {
            speed = characterProfiling.character.movementSpeed;
            animator = characterProfiling.character.characterPrefab.GetComponent<Animator>();
        }
    }
    // Start is called before the first frame update\
    void Start()
    {

        if (selectionBox != null) UnityEngine.Debug.Log("Successfully retrieved BoxSelector");

        movementObjects.Add(this);
    }

    Vector3 direction;
    Vector3 newPosition;

    // Update is called once per frame
    void Update()
    {
        float x = transform.position.x;
        float y = transform.position.y;

        //SelectMultipleCharacters();

        if (selected)
        {

            if (Input.GetKey(KeyCode.W))
            {
                y += speed / 10;
                //set animator
            }
            if (Input.GetKey(KeyCode.S))
            {
                y -= speed / 10;
            }
            if (Input.GetKey(KeyCode.A))
            {
                x -= speed / 10;
            }
            if (Input.GetKey(KeyCode.D))
            {
                x += speed / 10;
            }

            if (Input.GetMouseButtonUp(0) && selectScript.isActive) { 
                
                selected = isInsideSelectionBox ? true : false;
            }

            if (Input.GetMouseButton(1) && PartyController.orthoCamera.orthographic == true)
            {
                Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                mousePosition.z = transform.position.z; // Ensure the z-coordinate is the same as the character's z-position

                // Calculate the direction from the character's current position to the mouse position
                direction = (mousePosition - transform.position).normalized;

                // Calculate the new position based on the direction and speed
                newPosition = transform.position + direction * speed * Time.deltaTime;

                // Move the character to the new position
                transform.position = Vector3.Lerp(transform.position, newPosition, speed * Time.deltaTime);
            }
        }
        Vector3 relocatedPosition = new Vector3(x, y, transform.position.z);
        transform.position = Vector3.Lerp(transform.position, relocatedPosition, speed * Time.deltaTime);
    }

    private void OnMouseDown()
    {
        selected = true;

        foreach (MoveToMouse obj in movementObjects)
        {
            if (obj != this)
            {

                obj.selected = false;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if the character enters the selection box trigger
        if (collision.CompareTag("SelectionBox"))
        {
            isInsideSelectionBox = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        // Check if the character exits the selection box trigger
        if (collision.CompareTag("SelectionBox"))
        {
            isInsideSelectionBox = false;
        }
    }

}
