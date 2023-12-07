using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Photon.Pun;

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
    private void OnTriggerEnter2D(Collider2D other) {
        GameObject go = other.gameObject;
        HazardCheck(go);
    }
    private void OnCollisionEnter2D(Collision2D collision) {
        GameObject go = collision.gameObject;
        HazardCheck(go);
    }

    private void HazardCheck(GameObject go) {
        int dmg = 0;
        if (go.CompareTag("Spike"))
            dmg = 1;
        else if (go.CompareTag("Bomb"))
            dmg = 2;
        else if (go.CompareTag("Poison"))
            dmg = 3;

        if (dmg <= 0)
            return;
        TakeDamage(dmg);
        PhotonView pv = go.GetComponent<PhotonView>();
        if (!pv) {
            Destroy(go);
            return;
        }
        SelfCleanup sc = go.GetComponent<SelfCleanup>();
        if (sc)
            pv.RPC("NetworkRemove", pv.Owner);
        
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
