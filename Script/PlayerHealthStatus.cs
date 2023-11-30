using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerHealthStatus : MonoBehaviour
{
    public UnityEvent OnPlayerDeath;

    public int maxHealth = 10;
    public int currentHealth;

    public HealthBar healthBar;
    // Start is called before the first frame update
    void Start()
    {
        healthBar = FindObjectOfType<HealthBar>();
        if (!healthBar)
            Debug.LogError("No health bar");
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
    }

    // Update is called once per frame
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Spike"))
        {
            TakeDamage(1);
            Destroy(other.gameObject);
        }

        if (other.gameObject.CompareTag("Bomb"))
        {
            TakeDamage(2);
            Destroy(other.gameObject);
        }

        if (other.gameObject.CompareTag("Poison"))
        {
            TakeDamage(3);
            Destroy(other.gameObject);
        }
    }
    void TakeDamage(int damage)
    {
        currentHealth -= damage;

        if (currentHealth <= 0) {
            OnPlayerDeath.Invoke();
        }

        healthBar.SetHealth(currentHealth);
    }
    public void Heal(int health) {
        currentHealth += health;
        currentHealth = Mathf.Min(maxHealth, health);
        healthBar.SetHealth(currentHealth);
    }
}
