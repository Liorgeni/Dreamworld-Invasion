using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovements : MonoBehaviour
{
    // Rotation
    float mouseX;
    float mouseY;
    public float lookSpeed;
    float cameraRange;

    public Transform cameraTurn;

    // Movement
    CharacterController cc;

    public float moveSpeed = 10f;  // Base movement speed
    public float sprintSpeed = 20f; // Max speed while sprinting
    public float crouchSpeed = 5f;  // Speed while crouching
    public float currentSpeed;      // Speed at any given moment (moveSpeed, sprintSpeed, crouchSpeed)

    float xAxis;
    float zAxis;

    Vector3 localDirection;

    float radius;

    public LayerMask groundLayerMask;
    public Transform heightSphereAboveGround;

    public bool groundCheck;
    float gravity;
    Vector3 gravityMove;

    // Crouch Variables
    float standHeight = 1.8f;   // Default height when standing
    public float crouchHeight = 1f;  // Height when crouching
    private bool isCrouching = false;

    // Zoom Variables
    public Camera playerCamera;      // The camera to zoom in
    public float normalFOV = 60f;    // Default Field of View
    public float zoomFOV = 30f;      // Zoomed-in Field of View
    public float zoomSpeed = 10f;    // Speed of zoom transition

    void Start()
    {
        lookSpeed = 200f;
        cc = GetComponent<CharacterController>();
        currentSpeed = moveSpeed;
        groundCheck = false;
        radius = 0.5f;
        gravity = -9.81f;
        cc.height = standHeight;  // Set the character's initial height to standing

        // Set initial FOV of the camera
        playerCamera.fieldOfView = normalFOV;
    }

    void Update()
    {
        Rotation();
        Movement();
        CheckIfFall();
        Crouch();
        HandleZoom();

        if(Input.GetKey(KeyCode.Escape))
        {
            Application.Quit();
        }

    }

    void Rotation()
    {
        mouseX = Input.GetAxis("Mouse X") * lookSpeed * Time.deltaTime;
        transform.Rotate(0, mouseX, 0);

        mouseY = Input.GetAxis("Mouse Y") * lookSpeed * Time.deltaTime;
        cameraRange -= mouseY;
        cameraRange = Mathf.Clamp(cameraRange, -30, 30);
        cameraTurn.localRotation = Quaternion.Euler(cameraRange, 0, 0);
    }

    void Movement()
    {
        // Check if sprint key (Left Shift) is held down (and not crouching)
        if (Input.GetKey(KeyCode.LeftShift) && !isCrouching)
        {
            currentSpeed = sprintSpeed;  // Set speed to sprint speed
        }
        else if (isCrouching)
        {
            currentSpeed = crouchSpeed;  // Set speed to crouch speed
        }
        else
        {
            currentSpeed = moveSpeed;    // Return to normal speed
        }

        // Get input for movement
        xAxis = Input.GetAxis("Horizontal"); // Do not multiply by speed yet
        zAxis = Input.GetAxis("Vertical");

        // Calculate movement direction based on input
        localDirection = (transform.forward * zAxis + transform.right * xAxis).normalized;

        // Move the character based on current speed
        cc.Move(localDirection * currentSpeed * Time.deltaTime);
    }

    void CheckIfFall()
    {
        if (Physics.CheckSphere(heightSphereAboveGround.position, radius, groundLayerMask))
        {
            groundCheck = true;
        }
        else
        {
            groundCheck = false;
        }

        if (!groundCheck)
        {
            gravityMove.y += gravity * Time.deltaTime;
        }
        else
        {
            gravityMove.y = 0;
        }

        if (groundCheck && Input.GetButtonDown("Jump"))
        {
            gravityMove.y += 5;
        }

        cc.Move(gravityMove * Time.deltaTime);
    }

    void Crouch()
    {
        // Check if crouch key (Left Control) is pressed
        if (Input.GetKey(KeyCode.LeftControl))
        {
            if (!isCrouching)
            {
                cc.height = crouchHeight;  // Adjust the height to crouch height
                isCrouching = true;        // Set crouching to true
            }
        }
        else
        {
            if (isCrouching)
            {
                cc.height = standHeight;  // Reset to standing height
                isCrouching = false;      // Set crouching to false
            }
        }
    }

    void HandleZoom()
    {
        // Check if the right mouse button is pressed
        if (Input.GetMouseButton(1))
        {
            // Smoothly transition to the zoomed-in FOV
            playerCamera.fieldOfView = Mathf.Lerp(playerCamera.fieldOfView, zoomFOV, zoomSpeed * Time.deltaTime);
        }
        else
        {
            // Smoothly return to the normal FOV
            playerCamera.fieldOfView = Mathf.Lerp(playerCamera.fieldOfView, normalFOV, zoomSpeed * Time.deltaTime);
        }
    }
}
