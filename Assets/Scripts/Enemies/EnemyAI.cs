using UnityEngine;
using UnityEngine.AI;
using System.Collections;
public class EnemyAI : MonoBehaviour
{
    public NavMeshAgent agent; // Reference to the NavMeshAgent component
    public Transform[] waypoints; // Array of waypoints for the enemy to move between
    public EnemyData enemyData; // Reference to the EnemyData ScriptableObject

    public float attackRange = 2f; // Range at which the enemy can attack
    public float detectionRange = 15f; // Range at which the enemy detects the player
    public float attackCooldown = 2f; // Time between attacks in seconds

    private float lastAttackTime; // Tracks the last time the enemy attacked
    private int currentWaypointIndex = 0; // Track the current waypoint the enemy is moving towards

    private Animator anim;
    public Transform player; // Reference to the player's position

    void Start()
    {
        agent = GetComponent<NavMeshAgent>(); // Get the NavMeshAgent component attached to the enemy
        anim = GetComponent<Animator>();

        MoveToNextWaypoint(); // Start moving to the first waypoint
    }

    void Update()
    {
        if (!agent.enabled)
        {
            Debug.LogError("NavMeshAgent is disabled!");
            return;
        }

        // Check the distance between the player and the enemy
        float distanceToPlayer = Vector3.Distance(player.position, transform.position);

        // Player detected: chase the player
        if (distanceToPlayer <= detectionRange)
        {
            StopCoroutine("WaitAtWaypoint"); // Stop any running coroutine to ensure it doesn't interfere
            EngagePlayer(distanceToPlayer); // Call the EngagePlayer function to handle movement
        }
        else
        {
            // Player out of detection range: return to waypoints
            anim.SetBool("isAttacking", false); // Stop attacking

            // If the enemy is moving, play the move animation
            if (agent.velocity.magnitude > 0.1f)
            {
                anim.SetBool("isMoving", true); // Enable moving animation when agent is moving
            }
            else
            {
                anim.SetBool("isMoving", false); // Switch to idle when agent stops

                // Only move to the next waypoint if the enemy is idle and has stopped moving
                if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
                {
                    // Start a coroutine to wait at the waypoint for a short duration
                    if (!anim.GetBool("isMoving")) // Ensure the enemy is idle
                    {
                        StartCoroutine(WaitAtWaypoint(2f)); // Wait for 2 seconds before moving to the next waypoint
                    }
                }
            }
        }
    }

    // Coroutine to wait at the waypoint before moving to the next one
    IEnumerator WaitAtWaypoint(float waitTime)
    {
        // Wait for the idle animation to play for the specified time
        yield return new WaitForSeconds(waitTime);

        // After waiting, move to the next waypoint
        MoveToNextWaypoint();
    }

    // Function to engage the player (move towards and then attack if close enough)
    void EngagePlayer(float distanceToPlayer)
    {
        // Move toward the player if out of attack range
        if (distanceToPlayer > attackRange)
        {
            if (!anim.GetCurrentAnimatorStateInfo(0).IsName("Move")) // Only trigger move if not already moving
            {
                anim.SetTrigger("move"); // Trigger the move animation once
            }
            agent.SetDestination(player.position);

            // Stop attacking if the player is out of range
            anim.SetBool("isAttacking", false);
        }

        // If the enemy is close enough to attack
        if (distanceToPlayer <= attackRange && Time.time >= lastAttackTime + attackCooldown)
        {
            // Enemy is close enough to attack, start attacking
            AttackPlayer(); // Call the AttackPlayer function
            lastAttackTime = Time.time; // Update the time of the last attack
        }
    }

    // Move to the next waypoint in the array
    void MoveToNextWaypoint()
    {
        if (waypoints.Length == 0)
            return; // No waypoints set

        anim.SetBool("isMoving", true); // Set move to true when switching to the next waypoint
        agent.SetDestination(waypoints[currentWaypointIndex].position);

        currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
    }

    void AttackPlayer()
    {
        anim.SetBool("isAttacking", true); // Start attack animation

        Debug.Log("Enemy is attacking the player!");

        // Check if the player has a PlayerHealth script attached and deal damage
        PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
        if (playerHealth != null)
        {
            playerHealth.TakeDamage(enemyData.damage); // Deal damage to the player
        }
    }

    // Callback from the Animator when the attack animation finishes
    public void OnAttackAnimationEnd()
    {
        // Check if the player is still in range after the attack
        float distanceToPlayer = Vector3.Distance(player.position, transform.position);

        if (distanceToPlayer <= attackRange)
        {
            // Player is still in range, continue attacking
            anim.SetBool("isAttacking", true);
        }
        else
        {
            // Player out of range, go back to moving
            anim.SetBool("isAttacking", false);
            anim.SetTrigger("move"); // Resume moving if player is out of range
        }
    }
}
