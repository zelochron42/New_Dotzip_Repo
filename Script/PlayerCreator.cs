using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class PlayerCreator : MonoBehaviourPunCallbacks {
    [SerializeField] private GameObject PlayerPrefab;

    bool spawnReady = false;
    bool playerSpawned = false;
    private void Start() {

    }
    public override void OnJoinedRoom() {
        base.OnJoinedRoom();
        spawnReady = true;
    }
    void SpawnPlayer() {
        PhotonNetwork.Instantiate(PlayerPrefab.name, PlayerPrefab.transform.position, PlayerPrefab.transform.rotation);
    }


    void Update() {
        if (spawnReady && !playerSpawned) {
            playerSpawned = true;
            SpawnPlayer();
        }
    }
}
