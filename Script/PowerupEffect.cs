using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Reference to this script: https://www.youtube.com/watch?v=PkNRPOrtyls 
public abstract class PowerupEffect : ScriptableObject
{
    public abstract void Apply(GameObject target);
}
