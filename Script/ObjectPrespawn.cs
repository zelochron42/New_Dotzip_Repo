using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon;
using Photon.Pun;
/// <summary>
/// Object that spawns another prefab on the network and then destroys itself
/// This object should be spawned locally on the Admin's side, so that they can see it before the players
/// </summary>
public class ObjectPrespawn : MonoBehaviour
{
    [SerializeField] float delayTime = 1f;
    [SerializeField] GameObject spawnObject;
    void Start()
    {
        StartCoroutine("SpawnRoutine");
    }

    IEnumerator SpawnRoutine() {
        yield return new WaitForSeconds(delayTime);
        PhotonNetwork.Instantiate(spawnObject.name, transform.position, spawnObject.transform.rotation);
        Destroy(gameObject);
        yield break;
    }
}
