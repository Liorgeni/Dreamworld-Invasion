using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 100;
    private int currentHealth;  

    public HealthBar healthBar; 

    void Start()
    {
        currentHealth = maxHealth;

        if (healthBar != null)
        {
            healthBar.maxHealth = maxHealth;
            healthBar.health = currentHealth;
        }
    }


    private void Update()
    {
        //Debug.Log(currentHealth);

    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage; 
        Debug.Log("Player took " + damage + " damage. Current health: " + currentHealth);

        // Update the health bar
        if (healthBar != null)
        {
            healthBar.health = currentHealth;
        }

        if (currentHealth <= 0)
        {
            Die(); 
        }

    }
    public void IncreaseHealth(float amount)
    {
        currentHealth += Mathf.FloorToInt(amount);
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        if (healthBar != null)
        {
            healthBar.health = currentHealth; 
            healthBar.UpdateHealthBar();
        }

    }
    // Handle player death
    void Die()
    {
        Debug.Log("Player has died!");
        // Add death logic here (e.g., restart level, show game over screen)
        // For now, just disable the player object
        gameObject.SetActive(false);
    }

    public void Heal(int amount)
    {
        currentHealth = Mathf.Min(currentHealth + amount, maxHealth);
        Debug.Log("Player healed. Current health: " + currentHealth);

        if (healthBar != null)
        {
            healthBar.health = currentHealth;
        }
    }
}
