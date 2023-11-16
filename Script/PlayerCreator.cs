using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class PlayerCreator : MonoBehaviourPunCallbacks {
    [SerializeField] private GameObject PlayerPrefab;
    [SerializeField] GameObject healthbarPrefab;

    bool spawnReady = false;
    bool playerSpawned = false;
    private void Start() {
        if (FindObjectOfType<AdminPings>()) {
            playerSpawned = true;
        }
    }
    public override void OnJoinedRoom() {
        base.OnJoinedRoom();
        spawnReady = true;
    }
    void SpawnPlayer() {
        Canvas c = FindObjectOfType<Canvas>();
        Instantiate(healthbarPrefab, c.transform);
        PhotonNetwork.Instantiate(PlayerPrefab.name, PlayerPrefab.transform.position, PlayerPrefab.transform.rotation);
    }


    void Update() {
        if (spawnReady && !playerSpawned) {
            playerSpawned = true;
            SpawnPlayer();
        }
        if (PhotonNetwork.IsConnectedAndReady)
            spawnReady = true;
    }
}
