using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollower : MonoBehaviour
{
    // Start is called before the first frame update
    public float followSpeed = 2f;
    public float partyTravelSpeed = 60f;
    public Transform target;
    private float zoom;

    private void Start()
    {
        //zoom = 
    }
    // Update is called once per frame
    void Update()
    {
        // Calculate the new position by moving to the right
        Vector3 newPos = new Vector3(Camera.main.transform.position.x + (partyTravelSpeed * Time.deltaTime), Camera.main.transform.position.y, -10f);

        // Move the camera to the new position
        Camera.main.transform.position = Vector3.Lerp(transform.position, newPos, followSpeed * Time.deltaTime);

        // Rest of your code (zoom handling, etc.) can go here if needed
    }
}
