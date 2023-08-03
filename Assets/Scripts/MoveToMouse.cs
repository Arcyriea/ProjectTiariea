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

    private Vector3 target;
    public bool selected { get; private set; }
    private Vector2 startPos;
    private Rect selectionRect;

    // Start is called before the first frame update
    void Start()
    {
        CharacterProfiling characterProfiling = this.GetComponent<CharacterProfiling>();
        if (characterProfiling != null)
        {
            speed = characterProfiling.character.movementSpeed;
        }
        movementObjects.Add(this);
        target = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        float x = transform.position.x;
        float y = transform.position.y;

        SelectMultipleCharacters();

        if (selected)
        {

            if (Input.GetKey(KeyCode.W))
            {
                y += speed / 10;
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



            //if (Input.GetMouseButton(0))
            // {
            //     Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            //     mousePosition.z = transform.position.z; // Ensure the z-coordinate is the same as the character's z-position

            //     // Calculate the direction from the character's current position to the mouse position
            //     Vector3 direction = (mousePosition - transform.position).normalized;

            //     // Calculate the new position based on the direction and speed
            //     Vector3 newPosition = transform.position + direction * speed * Time.deltaTime;

            //     // Move the character to the new position
            //     transform.position = Vector3.Lerp(transform.position, newPosition, speed * Time.deltaTime);
            // }
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

    private void OnGUI()
    {
        if (Input.GetMouseButton(0))
        {
            // Draw the selection rectangle
            GUI.Box(selectionRect, GUIContent.none);
        }
    }


    private void SelectMultipleCharacters()
    {
        ///testing
        if (Input.GetMouseButton(0))
        {
            startPos = Input.mousePosition;
            selectionRect = new Rect(startPos.x, -startPos.y, 0, 0);
        }
        else if (Input.GetMouseButtonUp(0))
        {
            // Get the current mouse position
            Vector2 endPos = Input.mousePosition;

            // Calculate the dimensions of the selection rectangle
            selectionRect.width = endPos.x - startPos.x;
            selectionRect.height = -endPos.y - -startPos.y;

            // Iterate through all the movement objects
            foreach (MoveToMouse obj in movementObjects)
            {
                // Check if the object is within the selection rectangle
                if (selectionRect.Contains(obj.transform.position))
                {
                    // Set the object's selected state to true
                    obj.selected = true;
                }
            }
        }
        ///testing
    }
}
