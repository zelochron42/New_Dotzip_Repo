using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;

public class PlayerRoleManager : MonoBehaviourPunCallbacks
{
    [SerializeField] string gameScene;
    [SerializeField] int minimumPlayerCount;
    [SerializeField] List<Player> readyPlayers = new List<Player>();
    bool matchQueueing = false;

    void Awake()
    {
        /*if (!PhotonNetwork.IsMasterClient)
            enabled = false;*/
    }

    void Update()
    {
        if (PhotonNetwork.IsMasterClient && !matchQueueing && readyPlayers.Count >= PhotonNetwork.PlayerList.Length && readyPlayers.Count >= minimumPlayerCount) {
            matchQueueing = true;
            StartGame();
        }
    }

    [PunRPC]
    public void PlayerReady(Player readyPlayer) {
        if (!readyPlayers.Contains(readyPlayer)) {
            Debug.Log("Ready player " + readyPlayer.UserId);
            readyPlayers.Add(readyPlayer);
        }
        else {
            Debug.Log("Player already ready " + readyPlayer.UserId);
        }
    }

    public void StartGame() {
        SceneManager.LoadScene(gameScene);
    }

    public override void OnJoinedRoom() {
        base.OnJoinedRoom();
    }

    public override void OnLeftRoom() {
        base.OnLeftRoom();
    }

    
}
