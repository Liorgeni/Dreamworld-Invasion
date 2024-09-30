using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Campfire : MonoBehaviour
{
    public float healthIncreaseAmount = 10f;
    public float healthIncreaseInterval = 5f;
    public float detectionRadius = 5f;
    private bool isPlayerInRange = false;
    private PlayerHealth playerHealth; // Reference to the player's health script

    void Start()
    {
        playerHealth = FindObjectOfType<PlayerHealth>();
        StartCoroutine(IncreaseHealth());
    }

    void Update()
    {
        // Check if the player is within the campfire radius
        if (Vector3.Distance(transform.position, playerHealth.transform.position) <= detectionRadius)
        {
            isPlayerInRange = true;
        }
        else
        {
            isPlayerInRange = false;
        }
    }

    IEnumerator IncreaseHealth()
    {
        while (true)
        {
            yield return new WaitForSeconds(healthIncreaseInterval);

            if (isPlayerInRange)
            {
                playerHealth.IncreaseHealth(healthIncreaseAmount);
            }
        }
    }
}
