using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Reference to this script: https://www.youtube.com/watch?v=PkNRPOrtyls 
public class Powerup_ALT : MonoBehaviour
{
    public PowerupEffect powerupEffect;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Destroy(gameObject);
        powerupEffect.Apply(collision.gameObject);
    }
}
