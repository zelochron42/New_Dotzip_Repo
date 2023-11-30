using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Reference to this script: https://www.youtube.com/watch?v=PkNRPOrtyls 

[CreateAssetMenu(menuName = "Powerups/SpeedBuff")]
public class SpeedBuff : PowerupEffect
{
    public float amount;

    public override void Apply(GameObject target)
    {
        target.GetComponent<PlayerMovement>().moveSpeed += amount;
    }
}
