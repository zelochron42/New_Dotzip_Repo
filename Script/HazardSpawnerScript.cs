using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

/// <summary>
/// Script to randomly spawn hazards in different locations on the screen 
/// </summary>
public class HazardSpawnerScript : MonoBehaviour
{
    public GameObject[] hazards;
    int loopNumber = 0;
    void FixedUpdate()
    {
        if (loopNumber >= 12)
        {
            HazardSpawn();
            loopNumber = 0;
        }
        else
        {
            loopNumber++;
        }
    }


    void HazardSpawn()
    {
        int randomIndex = Random.Range(0, hazards.Length);

        Vector2 randomSpawnPoint = new Vector2(Random.Range(-10, 11), Random.Range(-10, 11));

        PhotonNetwork.Instantiate(hazards[randomIndex].name, randomSpawnPoint, Quaternion.identity);
    }
}