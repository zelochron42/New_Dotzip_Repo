using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Reference to this script: https://www.youtube.com/watch?v=PkNRPOrtyls 

[CreateAssetMenu(menuName = "Powerups/HealthBuff")]
public class HealthBuff : PowerupEffect
{
    public float amount;

    public override void Apply(GameObject target)
    {
        target.GetComponent<PlayerHealthStatus>().healthBar.value += amount;
    }
}
