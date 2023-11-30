using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

//Reference to this script: https://www.youtube.com/watch?v=PkNRPOrtyls 
public class Powerup_ALT : MonoBehaviour
{
    public PowerupEffect powerupEffect;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject go = collision.gameObject;
        ApplyEffect(go);
    }
    private void OnCollisionEnter2D(Collision2D collision) {
        GameObject go = collision.gameObject;
        ApplyEffect(go);
    }

    void ApplyEffect(GameObject go) {
        if (go.CompareTag("Player")) {
            powerupEffect.Apply(go);
            PhotonNetwork.Destroy(gameObject);
        }
    }
}
