using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;

public class Victory : MonoBehaviourPunCallbacks
{
    [SerializeField] string levelScene;
    RoundScorer scorer;
    bool started = false;
    private void Awake() {
        scorer = FindObjectOfType<RoundScorer>();
        if (!scorer) {
            Debug.LogError("Cannot find round scorer!");
            return;
        }

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player")) {
            PhotonView pv = collision.gameObject.GetComponent<PhotonView>();
            if (!pv || pv.Owner != PhotonNetwork.LocalPlayer)
                return;
            pv.RPC("DisableSelf", RpcTarget.All);
        }
        //Debug.Log("You Win");
        //SceneManager.LoadScene(levelScene);

    }
    void CheckGameover() { //checking to make sure all the players have either died or been set inactive before advancing the scene
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        if (!started) {
            if (players.Length > 0)
                started = true;
            else
                return;
        }
        foreach (GameObject p in players) {
            if (p.activeInHierarchy == true)
                return;
        }

        PhotonView scoreView = scorer.GetComponent<PhotonView>();
        if (scoreView)
            scoreView.RPC("RoundOver", PhotonNetwork.MasterClient);
        //TODO: add a hard-limit to level time to stop matches from going on indefinitely with one living player
    }

    private void Update() {
        CheckGameover();
    }
}
