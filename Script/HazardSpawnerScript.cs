using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

/// <summary>
/// Script to randomly spawn hazards in different locations on the screen 
/// </summary>
public class HazardSpawnerScript : MonoBehaviourPunCallbacks
{
    [SerializeField] bool debugMode;
    [System.Serializable]
    private class HazardData {
        [Tooltip("This object will be spawned regularly")]
        public GameObject prefab;
        [Tooltip("If true, objects only spawn for the admin"+
                "\n(ENABLE THIS FOR OBJECT PRESPAWNERS)")]
        public bool localSpawnOnly;
        [Tooltip("Base offset for spawned object from the camera's center")]
        public Vector2 fixedOffset;
        [Tooltip("Maximum X and Y values for randomizing object's position relative to camera")]
        public Vector2 randomOffset;
        [Tooltip("The maximum amount of fixed frames until the object spawns again")]
        public int maxSpawnDelay;
        [Tooltip("The percentage of frames that can be randomly skipped"+
                "\n(0% means objects spawn at a fixed rate, 100% means objects can randomly spawn with any delay below the max)")]
        [Range(0f, 100f)]
        public float delayRandomness;
        [HideInInspector]
        public int timeSinceLastSpawn = 0;
        [Tooltip("Uses gizmos to show the area this object can spawn in based on fixed and random offsets")]
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
            if (hd.timeSinceLastSpawn > hd.maxSpawnDelay) {
                hd.timeSinceLastSpawn = 0;
                if (hd.delayRandomness > 0f)
                    hd.timeSinceLastSpawn += (int)(Random.Range(0f, hd.maxSpawnDelay) * hd.delayRandomness / 100f);
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