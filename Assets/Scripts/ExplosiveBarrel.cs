using System.Collections.Generic;
using UnityEngine;

public class ExplosiveBarrel : MonoBehaviour
{
    public float explosionRadius = 5f;    // Radius of explosion
    public float explosionForce = 700f;   // Force of the explosion
    public int explosionDamage = 70;      // Damage dealt by the explosion
    public GameObject explosionEffect;    // Explosion particle effect
    public int barrelHealth = 10;         // Health of the barrel (new)
    private bool isExploded = false;      // Track if the barrel has exploded
    private List<GameObject> damagedObjects = new List<GameObject>(); // Track damaged objects
    private CoinsManager coinsManager;
    [SerializeField] private GameObject coinsGroup;

    private void Start()
    {
        coinsManager = FindObjectOfType<CoinsManager>();
    }

    // Call this method to trigger the explosion
    public void Explode()
    {
        if (isExploded) return; // Prevent multiple explosions
        isExploded = true; // Set flag to prevent further explosions

        // Show explosion effect
        if (explosionEffect != null)
        {
            Instantiate(explosionEffect, transform.position, transform.rotation);
        }

        // Reward coins
        if (coinsManager != null)
        {
            coinsManager.RewardCoin(10);
        }

        // Apply explosion force and damage to nearby objects
        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);
        foreach (Collider nearbyObject in colliders)
        {
            // Apply explosion force
            Rigidbody rb = nearbyObject.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.AddExplosionForce(explosionForce, transform.position, explosionRadius);
            }

            // Deal damage to players or enemies only once
            if (!damagedObjects.Contains(nearbyObject.gameObject)) // Check if already damaged
            {
                PlayerHealth playerHealth = nearbyObject.GetComponent<PlayerHealth>();
                if (playerHealth != null)
                {
                    playerHealth.TakeDamage(explosionDamage); // Deal damage to player
                    damagedObjects.Add(nearbyObject.gameObject); // Mark as damaged
                }

                EnemyHealth enemyHealth = nearbyObject.GetComponent<EnemyHealth>();
                if (enemyHealth != null)
                {
                    enemyHealth.TakeDamage(explosionDamage); // Deal damage to enemy
                    damagedObjects.Add(nearbyObject.gameObject); // Mark as damaged
                }
            }
        }

        // Destroy the barrel after the explosion
        Destroy(gameObject);
    }

    // Method to allow the barrel to take damage
    public void TakeDamage(int damage)
    {
        barrelHealth -= damage;
        if (barrelHealth <= 0)
        {
            Explode(); // Trigger explosion when health reaches 0
        }
    }
}
