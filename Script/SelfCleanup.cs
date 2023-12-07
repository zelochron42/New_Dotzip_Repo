using System.Collections;
using System.Collections.Generic;
using Photon.Realtime;
using Photon.Pun;
using UnityEngine;

/// <summary>
/// Script to go on any network objects that should self-terminate automatically after a set duration, like admin pings or temporary powerups/hazards
/// </summary>
public class SelfCleanup : MonoBehaviourPunCallbacks {
    [SerializeField] float lifetime = 1f;
    float timeAlive = 0f;
    bool isMine = false;
    public void SetLifetime(float life) {
        lifetime = life;
    }
    private void Awake() {
        isMine = photonView.IsMine;
    }
    void Update()
    {
        if (isMine)
            RemovalLoop();
    }

    void RemovalLoop() {
        timeAlive += Time.deltaTime;
        if (timeAlive >= lifetime) {
            PhotonNetwork.Destroy(gameObject);
        }
    }

    [PunRPC]
    public void NetworkRemove() {
        if (isMine)
            PhotonNetwork.Destroy(gameObject);
    }
}
