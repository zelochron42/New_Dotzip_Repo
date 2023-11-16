using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealthStatus : MonoBehaviour
{
    public int MaxHealth = 10;
    public int currentHealth;

    public HealthBar healthBar;
    // Start is called before the first frame update
    void Start()
    {
        healthBar = FindObjectOfType<HealthBar>();
        if (!healthBar)
            Debug.LogError("No health bar");
        currentHealth = MaxHealth;
        healthBar.SetMaxHealth(MaxHealth);
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
            TakeDamage(4);
            Destroy(other.gameObject);
        }
    }
    void TakeDamage(int damage)
    {
        currentHealth -= damage;

        healthBar.SetHealth(currentHealth);
    }
}
