using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider healthSlider;
    public Slider eashealthSlider;
    public float maxHealth = 100f;
    public float health;
    public float lerpSpeed = 0.05f;

    void Start()
    {
        health = maxHealth;
        healthSlider.maxValue = maxHealth;
        eashealthSlider.maxValue = maxHealth;
        UpdateHealthBar(); // Initialize the health bar to max health
    }

    void Update()
    {
        if (healthSlider.value != health)
        {
            healthSlider.value = health;
        }

        if (eashealthSlider.value != health)
        {
            eashealthSlider.value = Mathf.Lerp(eashealthSlider.value, health, lerpSpeed);
        }
    }

    public void UpdateHealthBar()
    {
        // Set the current health on the sliders
        healthSlider.value = health;
        eashealthSlider.value = health;
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        UpdateHealthBar(); // Update the health bar when taking damage
    }
}
