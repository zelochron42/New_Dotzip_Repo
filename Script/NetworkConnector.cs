using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class NetworkConnector : MonoBehaviourPunCallbacks, IConnectionCallbacks {
    string gameVersion = ".1";

    [SerializeField]
    private byte maxPlayersPerRoom = 5;

    // Start is called before the first frame update
    void Start() {
        PhotonNetwork.AutomaticallySyncScene = true;
        ConnectGame();
    }

    public void ConnectGame() {
        Debug.Log("Attempting connection");
        PhotonNetwork.GameVersion = gameVersion;
        PhotonNetwork.ConnectUsingSettings();
    }
    //Intial Photon Project Connection
    public override void OnConnectedToMaster() {
        Debug.Log("Connected to master server");
        PhotonNetwork.JoinRandomRoom();
    }

    
    //Triggers when player is disconnected
    public override void OnDisconnected(DisconnectCause cause) {
        Debug.Log("Oops! You got disconnected.");
    }

    public override void OnJoinRandomFailed(short returnCode, string message) {
        PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = maxPlayersPerRoom });
    }
    //Triggers when player enter a room within the server
    public override void OnPlayerEnteredRoom(Player newPlayer) {
        Debug.Log("Another player arrived.");
    }
    //triggers when player leaves a room within the server
    public override void OnPlayerLeftRoom(Player otherPlayer) {
        Debug.Log("Another player left.");
    }
}
