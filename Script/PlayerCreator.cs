using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class PlayerCreator : MonoBehaviourPunCallbacks {
    [SerializeField] private GameObject PlayerPrefab;
    [SerializeField] GameObject healthbarPrefab;
    [SerializeField] Canvas sceneCanvas;
    [SerializeField] bool spawnReady = false;
    [SerializeField] bool playerSpawned = false;
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
        //Canvas c = FindObjectOfType<Canvas>();
        Instantiate(healthbarPrefab, sceneCanvas.transform);
        Vector2 offset = new Vector2(Random.Range(-1.5f, 1.5f), Random.Range(-1.5f, 1.5f));
        Vector2 spawnPos = (Vector2)Camera.main.transform.position + offset;
        PhotonNetwork.Instantiate(PlayerPrefab.name, spawnPos, PlayerPrefab.transform.rotation);
    }


    void Update() {
        if (spawnReady && !playerSpawned) {
            playerSpawned = true;
            SpawnPlayer();
        }
        if (PhotonNetwork.IsConnectedAndReady && PhotonNetwork.InRoom)
            spawnReady = true;
    }
}
