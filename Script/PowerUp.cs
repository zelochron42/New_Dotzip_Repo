using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

// reference to the script: https://www.youtube.com/watch?v=CLSiRf_OrBk
public class PowerUp : MonoBehaviour
{
    public GameObject pickupEffect;

    public float multiplier = 10f;
    public float duration = 10f;
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(Pickup(other));
        }
    }

    IEnumerator Pickup(Collider2D player)
    {
        Instantiate(pickupEffect, transform.position, transform.rotation);
        Debug.Log("Pickup has been done.");

        PlayerHealthStatus status = player.GetComponent<PlayerHealthStatus>();
        status.maxHealth = (int)(status.maxHealth * multiplier); // internal float maxHealth was added in the PlayerHealthStatus script for it to work. Please add changes if needed - Devone

        GetComponent<MeshRenderer>().enabled = false;
        GetComponent<Collider2D>().enabled = false;

        yield return new WaitForSeconds(duration);
        status.maxHealth = (int)(status.maxHealth / multiplier);
        Destroy(gameObject);
    }
}
