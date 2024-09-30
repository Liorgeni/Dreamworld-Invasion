using UnityEngine;
using UnityEngine.AI; // Make sure you import NavMeshAgent

public class EnemyHealth : MonoBehaviour
{
    public EnemyData enemyData; // Reference to the EnemyData ScriptableObject
    private int currentHealth; // Store the current health of the enemy
    private Animator anim;
    private NavMeshAgent agent; // Reference to the NavMeshAgent component

    void Start()
    {
        // Initialize the enemy's health from the ScriptableObject
        currentHealth = enemyData.health;
        anim = GetComponent<Animator>(); 
        agent = GetComponent<NavMeshAgent>(); // Get the NavMeshAgent component
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage; // Reduce health based on damage taken
        Debug.Log(enemyData.enemyName + " took " + damage + " damage. Remaining health: " + currentHealth);
        anim.SetTrigger("wounded");

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        anim.SetTrigger("dies"); // Trigger death animation
        Debug.Log(enemyData.enemyName + " has died.");

        // Stop the NavMeshAgent from moving
        if (agent != null)
        {
            agent.isStopped = true; // Stop the agent's movement
            agent.velocity = Vector3.zero; // Reset any existing velocity
        }

        DisableEnemy(); // Disable enemy logic
    }

    void DisableEnemy()
    {
        // Disable any relevant components (like movement or AI scripts) so the enemy stops interacting
        GetComponent<EnemyAI>().enabled = false;
        GetComponent<Collider>().enabled = false;  // Disable the collider to prevent further interaction
    }
}
