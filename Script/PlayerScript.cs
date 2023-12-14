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
        UIManager canvasUI = FindObjectOfType<UIManager>();
        if (canvasUI) {
            gameOverCanvas = canvasUI.gameObject;
            gameOverCanvas.SetActive(false);
        }

        if (!photonView.IsMine) {
            PlayerMovement pm = GetComponent<PlayerMovement>();
            if (pm) {
                pm.enabled = false;
                Destroy(pm);
            }
            if (phs) {
                phs.enabled = false;
                Destroy(phs);
            }
        }
        else {
            //DontDestroyOnLoad(gameObject);
            if (phs)
                ConnectScorer(phs);
        }
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
            phs.OnPlayerDeath.RemoveAllListeners();
            if (scoreView)
                scoreView.RPC("PlayerDied", PhotonNetwork.MasterClient);
            if (gameOverCanvas)
                gameOverCanvas.SetActive(true);
            DestroyPlayer();
        });

    }

    [PunRPC]
    void DisableSelf() {
        gameObject.SetActive(false);
    }
    [PunRPC]
    private void DestroyPlayer() // reference for the script:
    {
        PhotonNetwork.Destroy(gameObject);
    }
}
