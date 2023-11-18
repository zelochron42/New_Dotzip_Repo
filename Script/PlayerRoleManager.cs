using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;
using TMPro;

public class PlayerRoleManager : MonoBehaviourPunCallbacks
{
    [SerializeField] string gameScene;
    [SerializeField] int minimumPlayerCount;
    [SerializeField] List<Player> readyPlayers = new List<Player>();
    [SerializeField] TextMeshProUGUI text;
    [SerializeField] GameObject adminObject;
    bool matchQueueing = false;
    PhotonView thisView;

    void Awake()
    {
        thisView = PhotonView.Get(this);
        /*if (!PhotonNetwork.IsMasterClient)
            enabled = false;*/
    }

    void Update()
    {
        if (PhotonNetwork.IsMasterClient && !matchQueueing) {
            QueueCheck();
        }
    }
    private void QueueCheck() {
        if (PhotonNetwork.IsMasterClient && readyPlayers.Count >= PhotonNetwork.PlayerList.Length && readyPlayers.Count >= minimumPlayerCount) {
            matchQueueing = true;
            StartGame();
        }
        SetAllText("Players Ready/Joined: " + readyPlayers.Count + "/" + PhotonNetwork.PlayerList.Length);
    }
    private void SetAllText(string txt) {
        thisView.RPC("SetText", RpcTarget.All, txt);
    }

    [PunRPC]
    private void SetText(string txt) {
        text.text = txt;
    }

    [PunRPC]
    private void PlayerReady(Player readyPlayer) {
        if (!readyPlayers.Contains(readyPlayer)) {
            Debug.Log("Ready player " + readyPlayer.UserId);
            readyPlayers.Add(readyPlayer);
        }
        else {
            Debug.Log("Player already ready " + readyPlayer.UserId);
        }
    }
    [PunRPC]
    private void PlayerUnready(Player readyPlayer) {
        if (readyPlayers.Contains(readyPlayer)) {
            readyPlayers.Remove(readyPlayer);
        }
    }

    [PunRPC]
    private void SetAdmin() {
        GameObject myAdmin = Instantiate(adminObject);
        DontDestroyOnLoad(myAdmin);
    }

    public void StartGame() {
        StartCoroutine("StartupRoutine");
    }

    IEnumerator StartupRoutine() {
        yield return new WaitForSeconds(0.25f);
        if (!CheckPlayerReady()) {
            matchQueueing = false;
            yield break;
        }
        SetAllText("Starting game...");
        yield return new WaitForSeconds(1f);

        SetAllText("Choosing admin...");
        Player admin = readyPlayers[Random.Range(0, readyPlayers.Count)];
        thisView.RPC("SetAdmin", admin);
        yield return new WaitForSeconds(1f);

        SetAllText("One moment please...");
        yield return new WaitForSeconds(1f);

        if (CheckPlayerReady())
            LoadGameScene();
        else {
            readyPlayers = new List<Player>();
            SetAllText("Unready players detected, cancelling game...");
            yield return new WaitForSeconds(2.5f);
            matchQueueing = false;
        }
        yield break;
    }

    public void LoadGameScene() {
        SceneManager.LoadScene(gameScene);
    }

    bool CheckPlayerReady() {
        bool unready = false;
        Player[] players = PhotonNetwork.PlayerList;
        foreach (Player p in players) {
            if (!readyPlayers.Contains(p)) {
                unready = true;
                readyPlayers = new List<Player>();
                break;
            }
        }
        return !unready;
    }
}
