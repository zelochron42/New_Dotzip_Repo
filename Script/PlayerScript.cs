using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

//this code ensures that players are only controlled by their owner client, and not by all other players
public class PlayerScript : MonoBehaviourPunCallbacks
{

    GameObject gameOverCanvas;
    PlayerHealthStatus playerHealth;
    //public static GameObject LocalPlayerInstance;

    void Awake()
    {
        PlayerHealthStatus phs = GetComponent<PlayerHealthStatus>();
        gameOverCanvas = FindObjectOfType<UIManager>().gameObject;
        gameOverCanvas.SetActive(false);

        if (!photonView.IsMine) {
            PlayerMovement pm = GetComponent<PlayerMovement>();
            if (pm)
                Destroy(pm);
            if (phs)
                Destroy(phs);
        }
        else if (phs) {
            ConnectScorer(phs);
        }
        DontDestroyOnLoad(gameObject);
    }

    void ConnectScorer(PlayerHealthStatus phs) {
        RoundScorer scorer = FindObjectOfType<RoundScorer>();
        if (!scorer) {
            Debug.LogError("Can't find round scorer!");
            return;
        }
        PhotonView scoreView = scorer.GetComponent<PhotonView>();
        if (!scoreView) {
            Debug.LogError("Can't find photon view on round scorer!");
            return;
        }
        scoreView.RPC("PlayerSpawned", PhotonNetwork.MasterClient);
        phs.OnPlayerDeath.AddListener(() => {
            scoreView.RPC("PlayerDied", PhotonNetwork.MasterClient);
            DestroyPlayer();
            gameOverCanvas.SetActive(true);
        });

    }

    [PunRPC]
    void DisableSelf() {
        gameObject.SetActive(false);
    }
    private void DestroyPlayer() // reference for the script:
    {
        Destroy(gameObject);
    }
}
