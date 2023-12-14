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
            scorer.GetComponent<PhotonView>().RPC("PlayerWon", PhotonNetwork.MasterClient);
            pv.RPC("DestroyPlayer", pv.Owner);
        }
    }
}
