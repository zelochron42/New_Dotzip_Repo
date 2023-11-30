using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

/// <summary>
/// Script to randomly spawn hazards in different locations on the screen 
/// </summary>
public class HazardSpawnerScript : MonoBehaviour
{
    [SerializeField] bool debugMode;
    [System.Serializable]
    private class HazardData {
        public GameObject prefab;
        public bool localSpawnOnly;
        public Vector2 fixedOffset;
        public Vector2 randomOffset;
        public int spawnDelay;
        [HideInInspector]
        public int timeSinceLastSpawn = 0;
        public bool displayGizmos;
    }
    bool adminClient = false;

    [SerializeField] HazardData[] hazardList;

    //public GameObject[] hazards;
    int loopNumber = 0;

    private void Start() {
        if (debugMode) {
            adminClient = true;
            return;
        }
        AdminPings ap = FindObjectOfType<AdminPings>();
        if (ap)
            adminClient = true;
        else
            Destroy(gameObject);
    }
    void FixedUpdate()
    {
        if (adminClient)
            SpawnHazardFromData();
        /*
        loopNumber++;
        if (loopNumber >= 12) {

            //HazardSpawn();
            loopNumber = 0;
        }*/
    }

    void SpawnHazardFromData() {
        Vector2 camPos = Camera.main.transform.position;
        foreach (HazardData hd in hazardList) {
            hd.timeSinceLastSpawn++;
            if (hd.timeSinceLastSpawn > hd.spawnDelay) {
                hd.timeSinceLastSpawn = 0;
                float XLimit = Mathf.Abs(hd.randomOffset.x);
                float YLimit = Mathf.Abs(hd.randomOffset.y);
                Vector2 randomizedOffset = new Vector2(Random.Range(-XLimit, XLimit), Random.Range(-YLimit, YLimit));
                Vector2 spawnOffset = hd.fixedOffset + randomizedOffset;
                if (hd.localSpawnOnly)
                    Instantiate(hd.prefab, camPos + spawnOffset, hd.prefab.transform.rotation);
                else
                    PhotonNetwork.Instantiate(hd.prefab.name, camPos + spawnOffset, hd.prefab.transform.rotation);
            }
        }
    }

    /*void HazardSpawn()
    {
        int randomIndex = Random.Range(0, hazards.Length);

        Vector2 randomSpawnPoint = new Vector2(Random.Range(-10, 11), Random.Range(-10, 11));

        PhotonNetwork.Instantiate(hazards[randomIndex].name, randomSpawnPoint, Quaternion.identity);
    }*/

    private void OnDrawGizmos() {
        Gizmos.color = Color.red;
        foreach (HazardData hd in hazardList) {
            if (hd.displayGizmos) {
                Gizmos.DrawWireCube((Vector2)Camera.main.transform.position + hd.fixedOffset, hd.randomOffset * 2f);
            }
        }
    }
}