using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayRaycast : MonoBehaviour
{

    Vector3 origin;
    RaycastHit hit;
    float maxDistance;
    public GameObject cameraPos;
    public LayerMask rayCastHitable;



    void Start()
    {
        maxDistance = 500f;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            Shoot();
        }

    }

    void Shoot()
    {
        //Physics.Raycast()

        origin = cameraPos.GetComponent<Camera>().ViewportToWorldPoint(new Vector3(.5f, .5f, 0));
        if (Physics.Raycast(origin, cameraPos.transform.forward, out hit, maxDistance, rayCastHitable))
        {

            hit.transform.GetComponent<MeshRenderer>().material.color = Color.red;
        }
    }

}
