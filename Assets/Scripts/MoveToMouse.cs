using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Security.Cryptography;
using UnityEngine;

public class MoveToMouse : MonoBehaviour
{
    public static List<MoveToMouse> movementObjects = new List<MoveToMouse>();
    public float speed;

    private Vector3 target;
    private bool selected;
    // Start is called before the first frame update
    void Start()
    {
        movementObjects.Add(this);
        target = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && selected)
        {
            target = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            target.z = transform.position.z;
        }
        transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);
    }

    private void OnMouseDown()
    {
        selected = true;

        foreach(MoveToMouse obj in movementObjects)
        {
            if (obj != this) { 
            
            obj.selected = false;
            }
        }
    }
}
