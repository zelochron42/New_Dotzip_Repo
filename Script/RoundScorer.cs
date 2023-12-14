using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(PhotonView))]
public class RoundScorer : MonoBehaviourPunCallbacks, IPunObservable {
    [SerializeField] string endScene;

    [SerializeField] int playersSpawned = 0;
    [SerializeField] int playerDeaths = 0;
    [SerializeField] int playerDisconnects = 0;
    [SerializeField] int playerVictories = 0;
    PhotonView scoreView;

    bool gameStarted = false;
    bool gameEnded = false;

    void Start() {
        DontDestroyOnLoad(gameObject);
        scoreView = GetComponent<PhotonView>();
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info) {
        if (stream.IsWriting) {
            stream.SendNext(new int[] { playersSpawned, playerDeaths, playerDisconnects, playerVictories });
        }
        else if (stream.IsReading) {
            int[] vs = (int[])stream.ReceiveNext();
            playersSpawned = vs[0];
            playerDeaths = vs[1];
            playerDisconnects = vs[2];
            playerVictories = vs[3];
        }
    }
    void Update() {
        if (!gameStarted && playersSpawned > 0)
            gameStarted = true;
        if (gameStarted && PhotonNetwork.IsMasterClient)
            CheckGameover();
    }
    [PunRPC]
    public void PlayerSpawned() {
        playersSpawned++;
    }
    [PunRPC]
    public void PlayerDied() {
        playerDeaths++;
    }
    [PunRPC]
    public void PlayerWon() {
        playerVictories++;
    }

    [PunRPC]
    void RoundOver() {
        if (!gameEnded) {
            gameEnded = true;
            StartCoroutine("EndDelay");
        }
    }

    IEnumerator EndDelay() {
        yield return new WaitForSeconds(1f);
        PhotonNetwork.LoadLevel(endScene);
        yield break;
    }

    void CheckGameover() { //checking to make sure all the players have either died or been set inactive before advancing the scene
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject p in players) {
            if (p.activeInHierarchy == true)
                return;
        }
        //if (playerDeaths + playerVictories >= playersSpawned)
        RoundOver();
        //TODO: add a hard-limit to level time to stop matches from going on indefinitely with one living player
    }

    public int[] GetGameData() {
        playerDisconnects = playersSpawned - (playerDeaths + playerVictories);
        return new int[] { playersSpawned, playerDeaths, playerDisconnects, playerVictories };
    }
}
