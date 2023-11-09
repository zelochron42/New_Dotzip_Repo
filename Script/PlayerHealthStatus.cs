using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealthStatus : MonoBehaviour
{
    public static event Action OnPlayerDeath; 
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
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            TakeDamage(1);
        }
    }
    void TakeDamage(int damage)
    {
        currentHealth -= damage;

        healthBar.SetHealth(currentHealth);

        // Display Game Over Screen
        // reference of this script: 
        OnPlayerDeath?.Invoke();
    }
}
