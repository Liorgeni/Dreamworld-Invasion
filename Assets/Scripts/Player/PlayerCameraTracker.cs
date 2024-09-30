using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCameraTracker : MonoBehaviour
{
    public Transform playerCamera;
    public float yOffeset = 5f;

    void Update()
    {
        Vector3 newPosition = playerCamera.position;
        newPosition.y = yOffeset;

        transform.position = newPosition;
    }
}
