using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraOrbit : MonoBehaviour
{
    public Transform target; // Target look object
    public bool hideCursor = false; // Is the cursor hidden?
    [Header("Settings")]
    public Vector3 offset;
    public Vector2 speed = new Vector2(120f, 120f); // X and Y speed of orbit (you can think of speed as sensitivity of the movement of the camera)
    public float minYLimit = 20f; // Limit in degrees of rotation of y
    public float maxYLimit = 80f;
    public float minDistance = .5f; // Limit in units of camera distance
    public float maxDistance = 15f;
    [Header("Collision")]
    public bool collision = false; // Is collision enabled?
    public float castRadius = .4f; // Radius of SphereCast
    public float rayDistance = 1000f; // Distance the ray travels
    public LayerMask ignoreLayers; // Layers to ignore from collision

    private Vector3 originalOffset; // Record the original offset on Start
    private float distance; // Current distance of camera
    private Vector3 euler; //x, y; // Current x and y rotation of camera (this could also be written as 'Vector2' since it's just x and y values)

    // Use this for initialization
    void Start()
    {
        // Does the cursor need hiding?
        if (hideCursor)
        {
            Cursor.lockState = CursorLockMode.Locked; // CursorLockMode is an enum, btw (0 = None, Locked = 1, Confined = 2)
            Cursor.visible = false;
        }
        else
        {
            // Unhide the cursor
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        // Calculate distance from target at Start
        originalOffset = transform.position - target.position;
        rayDistance = originalOffset.magnitude; // Use distance for ray

        // Get eulerAngles of camera and store them
        euler = transform.eulerAngles;
        /*Vector3 euler = transform.eulerAngles;
        x = euler.x;
        y = euler.y;*/

        // Set the initial distance
        distance = originalOffset.magnitude;
    }

    // Update is called once per frame
    void Update()
    {
        // If right mouse button is down
        if (Input.GetMouseButton(1))
        {
            // Get Mouse X and Mouse Y
            float mouseX = Input.GetAxis("Mouse X");
            float mouseY = Input.GetAxis("Mouse Y");
            // Call Look()
            Look(mouseX, mouseY);
        }
    }

    // Anything you want to happen AFTER 'Update', do it here (in LateUpdate)
    void LateUpdate() // Always use LateUpdate for cameras because you want them to render after every everything (objects, enemies, etc) has finished moving. Otherwise the camera would be jittery
    {
        // Is target valid (not null)?
        if (target)
        {
            // transform.TransformDirection()
            // Vector3 worldOffset = transform.TransformDirection(offset);
            transform.position = target.position - transform.forward * distance;
        }
    }
    // Rotate camera based on mouse X and Mouse Y
    public void Look(float mouseX, float mouseY)
    {
        // Is target valid (not null)?
        if (target)
        {
            // Modfify pitch
            euler.x -= mouseY * speed.x * Time.deltaTime;
            // Modify yaw
            euler.y += mouseX * speed.x * Time.deltaTime;          
            // Apply rotation (y first, x last)
            transform.rotation = Quaternion.Euler(euler.x, euler.y, 0);
        }
    }
}
