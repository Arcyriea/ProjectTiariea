using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionBox : MonoBehaviour
{
    private LineRenderer lineRenderer;
    private Vector2 initialMousePosition, currentMousePosition;
    private BoxCollider2D boxCollider;// Start is called before the first frame update
    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (PartyController.orthoCamera.orthographic == true)
        {
            if (Input.GetMouseButtonDown(0))
            {
                lineRenderer.positionCount = 5;
                initialMousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                lineRenderer.SetPosition(0, new Vector2(initialMousePosition.x, initialMousePosition.y));
                lineRenderer.SetPosition(1, new Vector2(initialMousePosition.x, initialMousePosition.y));
                lineRenderer.SetPosition(2, new Vector2(initialMousePosition.x, initialMousePosition.y));
                lineRenderer.SetPosition(3, new Vector2(initialMousePosition.x, initialMousePosition.y));
                lineRenderer.SetPosition(4, new Vector2(initialMousePosition.x, initialMousePosition.y));

                boxCollider = gameObject.AddComponent<BoxCollider2D>();
                boxCollider.isTrigger = true;
                boxCollider.offset = new Vector3(transform.position.x, transform.position.y, transform.position.z);
            }

            if (Input.GetMouseButton(0))
            {
                currentMousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                lineRenderer.SetPosition(0, new Vector2(initialMousePosition.x, initialMousePosition.y));
                lineRenderer.SetPosition(1, new Vector2(initialMousePosition.x, currentMousePosition.y));
                lineRenderer.SetPosition(2, new Vector2(currentMousePosition.x, currentMousePosition.y));
                lineRenderer.SetPosition(3, new Vector2(currentMousePosition.x, initialMousePosition.y));
                lineRenderer.SetPosition(4, new Vector2(initialMousePosition.x, initialMousePosition.y));

                transform.position = (currentMousePosition + initialMousePosition) / 2;

                boxCollider.size = new Vector2(
                    Mathf.Abs(initialMousePosition.x - currentMousePosition.x),
                    Mathf.Abs(initialMousePosition.y - currentMousePosition.y)
                    );
            }

            if (Input.GetMouseButtonUp(0))
            {
                lineRenderer.positionCount = 0;
                Destroy(boxCollider);
                transform.position = Vector3.zero;
            }
        }
        
    }

}
